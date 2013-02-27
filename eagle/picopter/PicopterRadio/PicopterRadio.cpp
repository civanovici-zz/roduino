/* Copyright (c) 2011 Frank Zhao
   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

   * Redistributions of source code must retain the above copyright
     notice, this list of conditions and the following disclaimer.
   * Redistributions in binary form must reproduce the above copyright
     notice, this list of conditions and the following disclaimer in the
     documentation and/or other materials provided with the distribution.
   * Neither the name of the authors nor the names of its contributors
     may be used to endorse or promote products derived from this software
     without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
   AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
   IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
   ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
   LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
   CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
   SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
   CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
   ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
   POSSIBILITY OF SUCH DAMAGE. */

/*
 * Please note, this file is designed to support both compilation with a C compiler and C++ compiler
*/

#include "PicopterRadio.h"
#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/eeprom.h>

#define PCR_FRAME_START_IDX 7
#define PCR_DATATYPE_FLIGHTCOMMAND 0
#define PCR_DATATYPE_SERIAL 1
#define PCR_DATATYPE_DISCOVER 2
#define PCR_DATATYPE_DISCOVER_REPLY 3

#define PCR_RX_FIFO_SIZE 128 // size for the RX FIFO ring buffer
#define PCR_TX_FIFO_SIZE (MAX_FRAME_SIZE - PCR_FRAME_START_IDX - 3 - 2 - 2) // size for the TX FIFO ring buffer
#define PCR_SERTX_FIFO_SIZE 128 // size for the serial TX FIFO ring buffer

#define PCR_CHAN_EEPROM_ADDR ((uint8_t *)(E2END - 5))

// these macros and pointers are designed to eliminate the overhead of function calls
#define RADIO_RX_FRAME_PTR    ((uint8_t*)&TRXFBST)
#define RADIO_RX_LENGTH       (TST_RX_LENGTH)
#define RADIO_RX_GETLENGTH()  (TST_RX_LENGTH)
#define RADIO_TX_FRAME_PTR    ((uint8_t*)&TRXFBST + 1)
#define RADIO_TX_SETLENGTH(x) TRXFBST = (x)
#define RADIO_STATUS_MASK     (_BV(TRX_CMD0) | _BV(TRX_CMD1) | _BV(TRX_CMD2) | _BV(TRX_CMD3) | _BV(TRX_CMD4))
#define PCR_PREPTX()          {\
                              TCNT4 = 0; tmr4_ovf_flag_tx = 0; tmpStatusReg = TRX_STATUS & RADIO_STATUS_MASK;\
                              while ((tmpStatusReg == BUSY_RX || tmpStatusReg == BUSY_TX || tmpStatusReg == STATE_TRANSITION_IN_PROGRESS) && tmr4_ovf_flag_tx < 1) tmpStatusReg = TRX_STATUS & RADIO_STATUS_MASK;\
                              TRX_STATE = CMD_PLL_ON;\
                              while (tmpStatusReg != PLL_ON) tmpStatusReg = TRX_STATUS & RADIO_STATUS_MASK;\
                              RADIO_TX_FRAME_PTR[0] = 0x01;\
                              RADIO_TX_FRAME_PTR[1] = 0x80;\
                              RADIO_TX_FRAME_PTR[2] = 0x00;\
                              RADIO_TX_FRAME_PTR[3] = 0x11;\
                              RADIO_TX_FRAME_PTR[4] = 0x22;\
                              RADIO_TX_FRAME_PTR[5] = 0x33;\
                              RADIO_TX_FRAME_PTR[6] = 0x44;\
                              }
#define PCR_FINALIZETX(x)     {\
                              RADIO_TX_FRAME_PTR[PCR_FRAME_START_IDX + (x)] = 0;\
                              RADIO_TX_FRAME_PTR[PCR_FRAME_START_IDX + (x) + 1] = 0;\
                              RADIO_TX_SETLENGTH(PCR_FRAME_START_IDX + (x) + 2);\
                              ZR_RFTX_LED_ON();\
                              TRX_STATE = CMD_TX_START;\
                              }
#define PCR_WAITTXDONE()      {TCNT4 = 0; tmr4_ovf_flag_tx = 0; tmpStatusReg = TRX_STATUS & RADIO_STATUS_MASK; while (tmr4_ovf_flag_tx < 1 && (tmpStatusReg != RX_ON && tmpStatusReg != PLL_ON)) tmpStatusReg = TRX_STATUS & RADIO_STATUS_MASK;}
#define PCR_ALLOWEDTX()       (tmr4_ovf_flag_rx1 >= pcr_waitThresh)
#define PCR_WAITFREEAIR()     {while (tmr4_ovf_flag_rx1 < pcr_waitThresh);}
#define PCR_SETRXMODE()       {TCNT4 = 0; tmr4_ovf_flag_tx = 0; tmpStatusReg = TRX_STATUS & RADIO_STATUS_MASK;\
                              while ((tmpStatusReg == BUSY_RX || tmpStatusReg == BUSY_TX || tmpStatusReg == STATE_TRANSITION_IN_PROGRESS) && tmr4_ovf_flag_tx < 1) tmpStatusReg = TRX_STATUS & RADIO_STATUS_MASK;\
                              TRX_STATE = CMD_RX_ON;\
                              }

// private function declarations
void pcr_replyToDiscovery();

uint8_t pcr_myDevAddr;

volatile uint8_t tmpStatusReg;

volatile char pcr_flushOnNext = 0;
volatile char pcr_hasRepliedToDiscover = 0;
volatile uint8_t tmr4_ovf_cnt = 0;
volatile uint8_t tmr4_ovf_flag_tx = 0xFF;
volatile uint8_t tmr4_ovf_flag_rx1 = 0xFF;
volatile uint8_t tmr4_ovf_flag_rx8 = 0xFF;
volatile uint8_t tmr4_ovf_cnt_rcrx = 0xFF;
uint8_t pcr_waitThresh;

// various fifos
volatile uint8_t pcr_rxRingBuffer[PCR_RX_FIFO_SIZE];
volatile uint8_t pcr_rxRingBufferHead = 0;
volatile uint8_t pcr_rxRingBufferTail = 0;
volatile uint8_t pcr_txRingBuffer[PCR_TX_FIFO_SIZE];
volatile uint8_t pcr_txRingBufferHead = 0;
volatile uint8_t pcr_txRingBufferTail = 0;

// stores incoming and outgoing RC channel data
volatile uint16_t pcr_rcChan[PCR_RCCHAN_CNT];

#ifdef __cplusplus
// single global instance, made available for use
cPicopterRadio PicopterRadio = cPicopterRadio();

/**
 * @brief Empty Constructor
 */
cPicopterRadio::cPicopterRadio()
{
}
#endif 

/**
 * @brief Initialization
 *
 * @param devId address identifier of this device
 * @param baudRate serial port baud rate
 */
#ifdef __cplusplus
void cPicopterRadio::begin(uint8_t devId)
#else
void pcr_begin(uint8_t devId)
#endif
{
    pcr_myDevAddr = devId;
    
    // note that radio_rfa.c has been heavily modified to suit our needs here
    radio_init(0, MAX_FRAME_SIZE);
    
    // set the channel
    uint8_t chan = eeprom_read_byte(PCR_CHAN_EEPROM_ADDR);
    radio_set_param(RP_CHANNEL((uint8_t)((chan % PCR_RFCHAN_MAX) + PCR_RFCHAN_START)));
    
    // set the datarate to 2 mbit/s
    radio_set_param(RP_DATARATE(OQPSK2000));
    
    // default to receiver
    radio_set_state(STATE_RX);
    
    // enable the appropriate LEDs, including PA_EXT
    #ifdef ENABLE_DIG3_DIG4
    trx_bit_write(SR_PA_EXT_EN, 1);
    #endif
    ZR_RFRX_LED_OUTPUT();
    ZR_RFTX_LED_OUTPUT();
    ZR_RFRX_LED_OFF();
    ZR_RFTX_LED_OFF();
    
    // setup the UART
    //UBRR0 = 25; // 38400
    UBRR0 = 8; // 115200
    UCSR0A = 0;
    UCSR0B |= _BV(RXEN0) | _BV(TXEN0) | _BV(RXCIE0);
    
    // start timer 4
    TCCR4A = 0x00; // turn everything off
    TCCR4B = 0x01; // prescaler 1
    TIMSK4 = _BV(TOIE4); // enable overflow interrupt
    TCNT4 = 0;
    
    // sets up some pins on the mating connector
    // so the two devices can know when they connect
    if (pcr_myDevAddr == PCR_CTRLER_DEVADDR)
    {
        DDRB &= ~_BV(2);
        DDRB |= _BV(1);
        PORTB |= _BV(2);
        PORTB &= ~_BV(1);
        
        pcr_waitThresh = 4; // controller has to wait longer before allow transmission
    }
    else
    {
        DDRB &= ~_BV(1);
        DDRB |= _BV(2);
        PORTB |= _BV(1);
        PORTB &= ~_BV(2);
        
        pcr_waitThresh = 1;
    }
}

/**
 * @brief Timer4 Overflow Interrupt
 *
 * sets some flags to indicate whether or not the overflow has occured
 */
ISR(TIMER4_OVF_vect)
{
    if (tmr4_ovf_cnt % 8 == 0)
    {
        tmr4_ovf_flag_tx = (tmr4_ovf_flag_tx < 128) ? (tmr4_ovf_flag_tx + 1) : tmr4_ovf_flag_tx;
        tmr4_ovf_flag_rx8 = (tmr4_ovf_flag_rx8 < 128) ? (tmr4_ovf_flag_rx8 + 1) : tmr4_ovf_flag_rx8;
        tmr4_ovf_cnt = 0;
    }
    
    tmr4_ovf_cnt++;
    tmr4_ovf_flag_rx1 = (tmr4_ovf_flag_rx1 < 128) ? (tmr4_ovf_flag_rx1 + 1) : tmr4_ovf_flag_rx1;
    tmr4_ovf_cnt_rcrx = (tmr4_ovf_cnt_rcrx < 128) ? (tmr4_ovf_cnt_rcrx + 1) : tmr4_ovf_cnt_rcrx;
}

/**
 * @brief Default RX Event Handler
 *
 * First checks if the current device is being addressed
 * If so, then check the data type
 * RC channel data, aka flight commands, are placed into rcChan
 * while serial data is placed into the RX FIFO
 * Also automatically responds to discovery commands
 * and handles received discovery replies
 */
ISR(TRX24_RX_END_vect)
{
    ZR_RFRX_LED_OFF();
    
    // reset timer
    TCNT4 = 0; tmr4_ovf_flag_rx8 = 0; tmr4_ovf_flag_rx1 = 0;
    
    uint8_t i, devAddr, dataType;
    
    devAddr = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX];
    
    // is the message for me?
    if (devAddr == pcr_myDevAddr)
    {
        uint8_t dataType = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX+1];
        if (dataType == PCR_DATATYPE_FLIGHTCOMMAND)
        {
            // read into temporary buffer while calculating checksum
            uint8_t checksum = 0;
            uint8_t b;
            uint16_t tempRead[PCR_RCCHAN_CNT];
            for (i = 0; i < PCR_RCCHAN_CNT; i++)
            {
                // little endian
                b = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX+2 + (i * 2)];
                tempRead[i] = b;
                checksum += b;
                b = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX+3 + (i * 2)];
                tempRead[i] |= b << 8;
                checksum += b;
            }
            
            // confirm checksum and only copy if valid
            if (RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX + 2 + (PCR_RCCHAN_CNT * 2)] == checksum)
            {
                for (i = 0; i < PCR_RCCHAN_CNT; i++)
                {
                    pcr_rcChan[i] = tempRead[i];
                }
                tmr4_ovf_cnt_rcrx = 0;
                
                // see if the command carried a payload of serial data
                uint8_t payLoadLen = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX + 3 + (PCR_RCCHAN_CNT * 2)];
                for (i = 0; i < payLoadLen; i++)
                {
                    uint8_t j = (pcr_rxRingBufferHead + 1) % PCR_RX_FIFO_SIZE;
                    if (j != pcr_rxRingBufferTail)
                    {
                        // push into FIFO
                        pcr_rxRingBuffer[pcr_rxRingBufferHead] = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX + 4 + (PCR_RCCHAN_CNT * 2) + i];
                        pcr_rxRingBufferHead = j;
                    }
                    else
                    {
                        // FIFO full
                        break;
                    }
                }
            }
            
            // if flush on next rx event is requested, quickly call flushTx
            if (pcr_flushOnNext)
            {
                tmr4_ovf_flag_rx1 = pcr_waitThresh + 1; // force immediate send with no wait
                pcr_flushTx();
                PCR_SETRXMODE();
                pcr_flushOnNext = 0; // reset the flag
            }
        }
        else if (dataType == PCR_DATATYPE_SERIAL)
        {
            uint8_t len = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX+2];
            for (i = 0; i < len; i++)
            {
                uint8_t j = (pcr_rxRingBufferHead + 1) % PCR_RX_FIFO_SIZE;
                if (j != pcr_rxRingBufferTail)
                {
                    // push into FIFO
                    pcr_rxRingBuffer[pcr_rxRingBufferHead] = RADIO_RX_FRAME_PTR[PCR_FRAME_START_IDX+3+i];
                    pcr_rxRingBufferHead = j;
                }
                else
                {
                    // FIFO full
                    break;
                }
            }
        }
        else if (dataType == PCR_DATATYPE_DISCOVER)
        {
            pcr_replyToDiscovery();
        }
        else if (dataType == PCR_DATATYPE_DISCOVER_REPLY)
        {
            pcr_hasRepliedToDiscover = 1;
        }
    }
    
    // reset timer
    TCNT4 = 0; tmr4_ovf_flag_rx8 = 0; tmr4_ovf_flag_rx1 = 0;
}

/**
 * @brief Discovery Reply
 *
 * Sends a reply to the incoming discovery request
 */
void pcr_replyToDiscovery()
{
    PCR_PREPTX();
    RADIO_TX_FRAME_PTR[PCR_FRAME_START_IDX+0] = (pcr_myDevAddr != PCR_CTRLER_DEVADDR) ? PCR_CTRLER_DEVADDR : PCR_FLIER_DEVADDR; // set payload
    RADIO_TX_FRAME_PTR[PCR_FRAME_START_IDX+1] = PCR_DATATYPE_DISCOVER_REPLY; // set payload
    PCR_FINALIZETX(2);
    PCR_WAITTXDONE();
    PCR_SETRXMODE();
}

/**
 * @brief Discovery Request
 *
 * Sends a discovery request to a remote device
 */
#ifdef __cplusplus
void cPicopterRadio::requestDiscovery()
#else
void pcr_requestDiscovery()
#endif
{
    pcr_hasRepliedToDiscover = 0;
    PCR_PREPTX();
    RADIO_TX_FRAME_PTR[PCR_FRAME_START_IDX+0] = (pcr_myDevAddr != PCR_CTRLER_DEVADDR) ? PCR_CTRLER_DEVADDR : PCR_FLIER_DEVADDR; // set payload
    RADIO_TX_FRAME_PTR[PCR_FRAME_START_IDX+1] = PCR_DATATYPE_DISCOVER; // set payload
    PCR_FINALIZETX(2);
    PCR_WAITTXDONE();
    PCR_SETRXMODE();
}

/**
 * @brief Discover Has Replied Check
 *
 * @return whether or not remote device has replied to discovery command
 */
#ifdef __cplusplus
uint8_t cPicopterRadio::hasDiscovered()
#else
uint8_t pcr_hasDiscovered()
#endif
{
    return pcr_hasRepliedToDiscover;
}

/**
 * @brief RX Buffer Flush
 *
 * Flush the RX FIFO ring buffer
 * see Arduino's Stream class
 */
#ifdef __cplusplus
void cPicopterRadio::flush()
#else
void pcr_flush()
#endif
{
    pcr_rxRingBufferHead = pcr_rxRingBufferTail;
}

/**
 * @brief RX Buffer Read
 *
 * pops and returns the next byte from the FIFO ring buffer
 * see Arduino's Stream class
 *
 * @return the next byte, or -1 if buffer is empty
 */
#ifdef __cplusplus
int cPicopterRadio::read()
#else
int pcr_read()
#endif
{
    // if the head isn't ahead of the tail, we don't have any characters
    if (pcr_rxRingBufferHead == pcr_rxRingBufferTail)
    {
        return -1;
    }
    else
    {
        uint8_t c = pcr_rxRingBuffer[pcr_rxRingBufferTail];
        pcr_rxRingBufferTail = (pcr_rxRingBufferTail + 1) % PCR_RX_FIFO_SIZE; // pop
        return c;
    }
}

/**
 * @brief RX Buffer Peek
 *
 * returns the next byte from the FIFO ring buffer without removing it
 * see Arduino's Stream class
 *
 * @return the next byte, or -1 if buffer is empty
 */
#ifdef __cplusplus
int cPicopterRadio::peek()
#else
int pcr_peek()
#endif
{
    // if the head isn't ahead of the tail, we don't have any characters
    if (pcr_rxRingBufferHead == pcr_rxRingBufferTail)
    {
        return -1;
    }
    else
    {
        uint8_t c = pcr_rxRingBuffer[pcr_rxRingBufferTail];
        return c;
    }
}

/**
 * @brief RX Buffer Size
 *
 * Shows how many bytes are in the RX FIFO ring buffer
 * see Arduino's Stream class
 *
 * @return how many bytes are in the RX FIFO ring buffer
 */
#ifdef __cplusplus
int cPicopterRadio::available()
#else
int pcr_available()
#endif
{
    int res = PCR_RX_FIFO_SIZE;
    res += pcr_rxRingBufferHead;
    res -= pcr_rxRingBufferTail;
    res %= PCR_RX_FIFO_SIZE;
    return res;
}

/**
 * @brief Flush Radio TX Buffer
 *
 * Sends all pending bytes in the TX FIFO buffer
 * Warning, does not return to RX mode automatically
 */
#ifdef __cplusplus
inline
#endif
void pcr_flushTx()
{
    if (pcr_txRingBufferHead != pcr_txRingBufferTail)
    {
        PCR_WAITFREEAIR();
        PCR_PREPTX();
        uint8_t dataLen = PCR_FRAME_START_IDX;
        RADIO_TX_FRAME_PTR[dataLen] = (pcr_myDevAddr != PCR_CTRLER_DEVADDR) ? PCR_CTRLER_DEVADDR : PCR_FLIER_DEVADDR; // set payload
        RADIO_TX_FRAME_PTR[dataLen+1] = PCR_DATATYPE_SERIAL; // set payload
        RADIO_TX_FRAME_PTR[dataLen+2] = pcr_txRingBufferHead;
        dataLen += 3;
        for (pcr_txRingBufferTail = 0; pcr_txRingBufferTail < pcr_txRingBufferHead; pcr_txRingBufferTail++, dataLen++)
        {
            RADIO_TX_FRAME_PTR[dataLen] = pcr_txRingBuffer[pcr_txRingBufferTail];
        }
        PCR_FINALIZETX(dataLen-PCR_FRAME_START_IDX);
    }
    pcr_txRingBufferHead = 0;
    pcr_txRingBufferTail = 0;
}
#ifdef __cplusplus
void cPicopterRadio::flushTx()
{
    pcr_flushTx();
}
#endif

/**
 * @brief TX a Byte
 *
 * Places byte to be sent into ring buffer first, and transmit when ring buffer is filled, or manually triggered
 *
 * @param c character to be sent
 */
#ifdef __cplusplus
inline
#endif
void pcr_write(uint8_t c)
{
    uint8_t j = pcr_txRingBufferHead + 1;
    if (j < PCR_RX_FIFO_SIZE)
    {
        // push into FIFO
        pcr_txRingBuffer[pcr_txRingBufferHead] = c;
        pcr_txRingBufferHead = j;
    }
    else
    {
        // FIFO really full
        
        pcr_flushTx();
        PCR_SETRXMODE();
        pcr_txRingBuffer[pcr_txRingBufferHead] = c;
        pcr_txRingBufferHead++;
    }
}
#ifdef __cplusplus
void cPicopterRadio::write(uint8_t c)
{
    pcr_write(c);
}
#endif

/**
 * @brief Default TX Complete Event Handler
 *
 * Clear the TX busy status flag
 */
ISR(TRX24_TX_END_vect)
{
    ZR_RFTX_LED_OFF();
}

/**
 * @brief Send RC Channel Data
 *
 * Send the data stored inside pcr_rcChan over using the radio
 * Warning, does not return to RX mode
 */
#ifdef __cplusplus
void cPicopterRadio::sendFlightCommands()
#else
void pcr_sendFlightCommands()
#endif
{
    uint8_t checksum = 0;
    uint8_t b;
    PCR_WAITFREEAIR();
    PCR_PREPTX();
    uint8_t dataLen = PCR_FRAME_START_IDX;
    RADIO_TX_FRAME_PTR[dataLen] = (pcr_myDevAddr != PCR_CTRLER_DEVADDR) ? PCR_CTRLER_DEVADDR : PCR_FLIER_DEVADDR; // set payload
    RADIO_TX_FRAME_PTR[dataLen+1] = PCR_DATATYPE_FLIGHTCOMMAND; // set payload
    dataLen += 2;
    for (uint8_t i = 0; i < PCR_RCCHAN_CNT; i++, dataLen += 2)
    {
        // little endian
        b = pcr_rcChan[i] & 0xFF;
        checksum += b;
        RADIO_TX_FRAME_PTR[dataLen] = b;
        b = (pcr_rcChan[i] & 0xFF00) >> 8;
        checksum += b;
        RADIO_TX_FRAME_PTR[dataLen+1] = b;
    }
    RADIO_TX_FRAME_PTR[dataLen] = checksum;
    
    // tack on serial payload
    RADIO_TX_FRAME_PTR[dataLen+1] = pcr_txRingBufferHead;
    dataLen += 2;
    for (pcr_txRingBufferTail = 0; pcr_txRingBufferTail < pcr_txRingBufferHead; pcr_txRingBufferTail++, dataLen++)
    {
        RADIO_TX_FRAME_PTR[dataLen] = pcr_txRingBuffer[pcr_txRingBufferTail];
    }
    PCR_FINALIZETX(PCR_RCCHAN_CNT*2+3+1+pcr_txRingBufferHead);
    
    pcr_txRingBufferHead = 0;
    pcr_txRingBufferTail = 0;
}

#ifdef __cplusplus
/**
 * @brief Sets Radio Channel
 *
 * changes the radio channel by setting the radio channel state
 *
 * @param chan channel number, 11 to 26
 */
void cPicopterRadio::setChannel(channel_t chan)
{
    radio_set_param(RP_CHANNEL(chan));
    eeprom_update_byte(PCR_CHAN_EEPROM_ADDR, chan - PCR_RFCHAN_START);
}
#endif

/**
 * @brief Radio RX Started Event
 *
 * during this time, do not change radio states
 */
ISR(TRX24_RX_START_vect)
{
    ZR_RFRX_LED_ON();
    
    // reset timer
    TCNT4 = 0; tmr4_ovf_flag_rx8 = 0; tmr4_ovf_flag_rx1 = 0;
}

/**
 * @brief Radio Error Handler
 *
 * this can happen if setting the state of the radio failed
 */
#ifdef __cplusplus
extern "C" {
#endif
void radio_error(radio_error_t err)
{
    PCR_SETRXMODE();
}
#ifdef __cplusplus
} // extern "C"
#endif

inline void pcr_setRxMode()
{
    PCR_SETRXMODE();
}

#ifdef __cplusplus
/**
 * @brief Sets Radio to RX Mode
 */
void cPicopterRadio::setRxMode()
{
    PCR_SETRXMODE();
}
#endif

/**
 * @brief Generate Random Number
 *
 * Uses the RND_VALUE inside PHY_RSSI to generate a 8 bit random number
 *
 * return random 8 bit number
 */
#ifdef __cplusplus
uint8_t cPicopterRadio::rand()
#else
uint8_t pcr_rand()
#endif
{
    PCR_SETRXMODE();
    uint8_t r = 0;
    r |= (PHY_RSSI & 0x60) >> 5;
    _delay_us(2);
    r |= (PHY_RSSI & 0x60) >> 3;
    _delay_us(2);
    r |= (PHY_RSSI & 0x60) >> 1;
    _delay_us(2);
    r |= (PHY_RSSI & 0x60) << 1;
    return r;
}

/**
 * @brief Check If Radio is Active
 *
 * Checks if the remote device is present by judging the time since last receive event
 *
 * return 1 if last received is recent, 0 if not
 */
#ifdef __cplusplus
uint8_t cPicopterRadio::isPresent()
#else
uint8_t pcr_isPresent()
#endif
{
    return (tmr4_ovf_cnt_rcrx < 32) ? 1 : 0;
}

/**
 * @brief Set Flush TX on Next RX Event
 *
 * Will cause the next RC RX event to automatically call flushTx
 */
#ifdef __cplusplus
void cPicopterRadio::setFlushOnNext()
#else
void pcr_setFlushOnNext()
#endif
{
    pcr_flushOnNext = 1;
}