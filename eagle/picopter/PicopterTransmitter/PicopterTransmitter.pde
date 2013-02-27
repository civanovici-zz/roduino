#include <PicopterRadio.h>
#include <PicopterWiiClassicCtrler.h>

//#define USE_ACCUM_THROTTLE
//#define DEADBAND (128/8)

long lastMicros;
char heatBeatCnt;
char isArmed = 0;
uint16_t accumThrottle = 0;

void setup()
{
  // this sets up all the LEDs
  DDRF |= _BV(2) | _BV(3) | _BV(4);
  PORTF &= ~(_BV(2) | _BV(3) | _BV(4));
  
  Serial.begin(115200);
  Serial1.begin(115200);
  PicopterRadio.begin(PCR_CTRLER_DEVADDR);
  BATMON = 0x13;
  
  wcc_init();
  
  dual_stick_info_t data;
    
  if (!wcc_read(&data))
  {
    isp_setup();
  }
  
  lastMicros = micros();
}

void loop()
{
  long curMicros = micros();
  if ((curMicros - lastMicros) > (15 * 1000))
  {
    dual_stick_info_t data;
    
    if (wcc_read(&data))
    {
      pcr_rcChan[0] = 1500 + data.rx * 3;
      pcr_rcChan[1] = 1500 + data.ry * 3;
      
      #ifdef DEADBAND
      if (data.lx < -DEADBAND) {
        data.lx += DEADBAND;
      }
      else if (data.lx > DEADBAND) {
        data.lx -= DEADBAND;
      }
      else {
        data.lx = 0;
      }
      #endif
      
      #ifdef USE_ACCUM_THROTTLE
      if (isArmed)
      {
        #ifdef DEADBAND
        if (data.ly < -DEADBAND) {
          data.ly += DEADBAND;
        }
        else if (data.ly > DEADBAND) {
          data.ly -= DEADBAND;
        }
        else {
          data.ly = 0;
        }
        #endif
        accumThrottle += data.ly;
        accumThrottle = accumThrottle < 8000 ? 8000 : (accumThrottle > 16000 ? 16000 : accumThrottle);
      }
      else
      {
        accumThrottle = 1000 * 8;
      }
      pcr_rcChan[3] = accumThrottle / 8;
      #else
      pcr_rcChan[3] = 1350 + data.ly * 4;
      #endif
      
      pcr_rcChan[2] = 1500 + data.lx * 3;
      pcr_rcChan[4] = data.buttons;
      pcr_rcChan[5] = pcr_rcChan[4] ^ 0xAA55;
      
      if (data.buttons & (1 << PCC_SYNCCHAN))
      {
        // send new channel command
        Serial.print("C");
        uint8_t chan = PicopterRadio.rand();
        Serial.print(chan, DEC);
        Serial.print("\0");
        
        // wait for acknowledgement
        uint8_t blinkCnt = 0;
        while ((Serial.available() == 0 || blinkCnt < 5) && blinkCnt < 10)
        {
          PCS_LED2_ON();
          delay(100);
          PCS_LED2_OFF();
          delay(100);
          blinkCnt++;
        }
        
        if (Serial.available())
        {
          if (Serial.read() == 'C')
          {
            // change channel if acknowledged
            PicopterRadio.setChannel(PCR_RFCHAN_START + (chan % PCR_RFCHAN_MAX));
          }
        }
      }
      else if (data.buttons & (1 << PCC_WIICALI))
      {
        PCS_LED1_OFF();
        
        Serial1.print(pcr_rcChan[0]);
        Serial1.print(", ");
        Serial1.print(pcr_rcChan[1]);
        Serial1.print(", ");
        Serial1.print(pcr_rcChan[2]);
        Serial1.print(", ");
        Serial1.print(pcr_rcChan[3]);
        Serial1.print(", 0x");
        Serial1.print(pcr_rcChan[4], HEX);
        Serial1.print(", 0x");
        Serial1.print(pcr_rcChan[5], HEX);
        Serial1.println();
        
        wcc_initCalibrate();
        for (int i = 0; i < 5; i++)
        {
          PCS_LED2_ON();
          delay(100);
          PCS_LED2_OFF();
          delay(100);
        }
      }
      else
      {
        if (data.buttons & (1 << PCC_DISARM))
        {
          isArmed = 0;
        }
        
        if (data.buttons & (1 << PCC_ARM))
        {
          isArmed = 1;
        }
        
        PicopterRadio.sendFlightCommands();
        PicopterRadio.setRxMode();
        
        heatBeatCnt++;
        if (heatBeatCnt % (isArmed ? 2 : 4) == 0)
        {
          PCS_LED1_TOG();
        }
        if (heatBeatCnt % 4 == 0)
        {
          PCS_LED2_OFF();
        }
      }
    }
    else
    {
      for (int i = 0; i < 5; i++)
      {
        PCS_LED1_ON();
        PCS_LED2_ON();
        delay(100);
        PCS_LED1_OFF();
        PCS_LED2_OFF();
        delay(100);
      }
    }
    
    lastMicros = curMicros;
  }
  else
  {
    if (PicopterRadio.isPresent())
    {
      PCS_LED2_ON();
    }
    else
    {
      PCS_LED2_OFF();
    }
  }
  
  // check for low battery
  if ((BATMON & (1 << BATMON_OK)) == 0)
  {
    // freeze code forever and blink LEDs
    while (1)
    {
      PCS_LED1_ON();
      delay(50);
      PCS_LED1_OFF();
      delay(500);
    }
  }
  
  if (Serial1.available()) {
    while (Serial1.available()) {
      PicopterRadio.write((char)Serial1.read());
    }
    heatBeatCnt = 0;
  }
  
  if (PicopterRadio.available()) {
    while (PicopterRadio.available()) {
      Serial1.write((char)PicopterRadio.read());
    }
    PCS_LED2_TOG();
    heatBeatCnt = 0;
  }
}
