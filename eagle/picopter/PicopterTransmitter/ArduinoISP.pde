// Feb 2012 edited by Frank26080115 for PalmQuad

// October 2009 by David A. Mellis
// - Added support for the read signature command
// 
// February 2009 by Randall Bohn
// - Added support for writing to EEPROM (what took so long?)
// Windows users should consider WinAVR's avrdude instead of the
// avrdude included with Arduino software.
//
// January 2008 by Randall Bohn
// - Thanks to Amplificar for helping me with the STK500 protocol
// - The AVRISP/STK500 (mk I) protocol is used in the arduino bootloader
// - The SPI functions herein were developed for the AVR910_ARD programmer 
// - More information at http://code.google.com/p/mega-isp

#define HWVER 2
#define SWMAJ 1
#define SWMIN 18

// STK Definitions
#define STK_OK 0x10
#define STK_FAILED 0x11
#define STK_UNKNOWN 0x12
#define STK_INSYNC 0x14
#define STK_NOSYNC 0x15
#define CRC_EOP 0x20 //ok it is a space...

void isp_setup() {
  DDRF |= _BV(2) | _BV(3); // LEDs as output
  int i;
  for (i = 0; i < 10; i++)
  {
    PORTF ^= _BV(2) | _BV(3); // flash LEDs to indicate start of mode
    delay(100);
  }
  
  autoerase = 0;
  
  UCSR1B &= ~_BV(RXCIE1);
  
  while(1) isp_loop();
}

int error=0;
int pmode=0;
// address for reading and writing, set by 'U' command
int here;
uint8_t buff[256]; // global block storage
char autoerase;

#define beget16(addr) (*addr * 256 + *(addr+1) )
typedef struct param {
  uint8_t devicecode;
  uint8_t revision;
  uint8_t progtype;
  uint8_t parmode;
  uint8_t polling;
  uint8_t selftimed;
  uint8_t lockbytes;
  uint8_t fusebytes;
  int flashpoll;
  int eeprompoll;
  int pagesize;
  int eepromsize;
  int flashsize;
} 
parameter;

parameter param;  

void isp_loop(void) {
  // is pmode active?
  if (pmode) PORTF |= _BV(2);
  else       PORTF &= ~_BV(2);
  // is there an error
  if (error) PORTF &= ~_BV(3);
  else       PORTF |= _BV(3);
  
  if ((UCSR1A & _BV(RXC1))) {
    avrisp();
  }
}

uint8_t getch() {
  while(!(UCSR1A & _BV(RXC1)));
  PORTF ^= _BV(3);
  return UDR1;
}

void readbytes(int n) {
  for (int x = 0; x < n; x++) {
    buff[x] = getch();
  }
}

void putch(char c) {
  while (!(UCSR1A & _BV(UDRE1)));
  PORTF ^= _BV(3);
  UDR1 = c;
}

void spi_init() {
  uint8_t x;
  SPCR = 0x53;
  x=SPSR;
  x=SPDR;
}

uint8_t spi_send(uint8_t b) {
  uint8_t reply;
  SPDR=b;
  loop_until_bit_is_set(SPSR, SPIF);
  reply = SPDR;
  return reply;
}

uint8_t spi_transaction(uint8_t a, uint8_t b, uint8_t c, uint8_t d) {
  uint8_t n;
  spi_send(a); 
  n=spi_send(b);
  //if (n != a) error = -1;
  n=spi_send(c);
  return spi_send(d);
}

void empty_reply() {
  if (CRC_EOP == getch()) {
    putch((char)STK_INSYNC);
    putch((char)STK_OK);
  } 
  else {
    putch((char)STK_NOSYNC);
  }
}

void breply(uint8_t b) {
  if (CRC_EOP == getch()) {
    putch((char)STK_INSYNC);
    putch((char)b);
    putch((char)STK_OK);
  } 
  else {
    putch((char)STK_NOSYNC);
  }
}

void get_version(uint8_t c) {
  switch(c) {
  case 0x80:
    breply(HWVER);
    break;
  case 0x81:
    breply(SWMAJ);
    break;
  case 0x82:
    breply(SWMIN);
    break;
  case 0x93:
    breply('S'); // serial programmer
    break;
  default:
    breply(0);
  }
}

void set_parameters() {
  // call this after reading paramter packet into buff[]
  param.devicecode = buff[0];
  param.revision = buff[1];
  param.progtype = buff[2];
  param.parmode = buff[3];
  param.polling = buff[4];
  param.selftimed = buff[5];
  param.lockbytes = buff[6];
  param.fusebytes = buff[7];
  param.flashpoll = buff[8]; 
  // ignore buff[9] (= buff[8])
  //getch(); // discard second value
  
  // WARNING: not sure about the byte order of the following
  // following are 16 bits (big endian)
  param.eeprompoll = beget16(&buff[10]);
  param.pagesize = beget16(&buff[12]);
  param.eepromsize = beget16(&buff[14]);

  // 32 bits flashsize (big endian)
  param.flashsize = buff[16] * 0x01000000
    + buff[17] * 0x00010000
    + buff[18] * 0x00000100
    + buff[19];

}

void start_pmode() {
  spi_init();
  // following delays may not work on all targets...
  DDRB |= _BV(0); // SS pin as output
  DDRE |= _BV(2); // reset pin as output
  PORTE |= _BV(2); // reset pin high
  DDRB |= _BV(1); // SCK pin as output
  PORTB &= ~_BV(1); // SCK pin low
  delay(50);
  PORTE &= ~_BV(2); // reset pin low
  delay(50);
  DDRB &= ~_BV(3); // MISO pin as input
  DDRB |= _BV(2); // MOSI pin as output
  spi_transaction(0xAC, 0x53, 0x00, 0x00);
  pmode = 1;
}

void end_pmode() {
  // relevant pins as input
  DDRE &= ~_BV(2);
  PORTE &= ~_BV(2);
  DDRB &= ~_BV(1);
  PORTB &= ~_BV(2);
  DDRB &= ~_BV(3);
  PORTB &= ~_BV(1);
  DDRB &= ~_BV(2);
  PORTB &= ~_BV(3);
  pmode = 0;
  autoerase = 0;
}

void universal() {
  int w;
  uint8_t ch;

  for (w = 0; w < 4; w++) {
    buff[w] = getch();
  }
  ch = spi_transaction(buff[0], buff[1], buff[2], buff[3]);
  breply(ch);
}

void flash(uint8_t hilo, int addr, uint8_t data) {
  spi_transaction(0x40+8*hilo, 
  addr>>8 & 0xFF, 
  addr & 0xFF,
  data);
}
void commit(int addr) {
  spi_transaction(0x4C, (addr >> 8) & 0xFF, addr & 0xFF, 0);
}

//#define _current_page(x) (here & 0xFFFFE0)
int current_page(int addr) {
  if (param.pagesize == 32) return here & 0xFFFFFFF0;
  if (param.pagesize == 64) return here & 0xFFFFFFE0;
  if (param.pagesize == 128) return here & 0xFFFFFFC0;
  if (param.pagesize == 256) return here & 0xFFFFFF80;
  return here;
}
uint8_t write_flash(int length) {
  if (param.pagesize < 1) return STK_FAILED;
  //if (param.pagesize != 64) return STK_FAILED;
  int page = current_page(here);
  int x = 0;
  while (x < length) {
    if (page != current_page(here)) {
      commit(page);
      page = current_page(here);
    }
    flash(LOW, here, buff[x++]);
    flash(HIGH, here, buff[x++]);
    here++;
  }

  commit(page);

  return STK_OK;
}

uint8_t write_eeprom(int length) {
  // here is a word address, so we use here*2
  // this writes byte-by-byte,
  // page writing may be faster (4 bytes at a time)
  for (int x = 0; x < length; x++) {
    spi_transaction(0xC0, 0x00, here*2+x, buff[x]);
    delay(45);
  } 
  return STK_OK;
}

void program_page() {
  char result = (char) STK_FAILED;
  int length = 256 * getch();
  length += getch();
  if (length > 256) {
      putch((char) STK_FAILED);
      return;
  }
  char memtype = getch();
  for (int x = 0; x < length; x++) {
    buff[x] = getch();
  }
  if (CRC_EOP == getch()) {
    putch((char) STK_INSYNC);
    if (memtype == 'F') result = (char)write_flash(length);
    if (memtype == 'E') result = (char)write_eeprom(length);
    putch(result);
  } 
  else {
    putch((char) STK_NOSYNC);
  }
}
uint8_t flash_read(uint8_t hilo, int addr) {
  return spi_transaction(0x20 + hilo * 8,
    (addr >> 8) & 0xFF,
    addr & 0xFF,
    0);
}

char flash_read_page(int length) {
  for (int x = 0; x < length; x+=2) {
    uint8_t low = flash_read(LOW, here);
    putch((char) low);
    uint8_t high = flash_read(HIGH, here);
    putch((char) high);
    here++;
  }
  return STK_OK;
}

char eeprom_read_page(int length) {
  // here again we have a word address
  for (int x = 0; x < length; x++) {
    uint8_t ee = spi_transaction(0xA0, 0x00, here*2+x, 0xFF);
    putch((char) ee);
  }
  return STK_OK;
}

void read_page() {
  char result = (char)STK_FAILED;
  int length = 256 * getch();
  length += getch();
  char memtype = getch();
  if (CRC_EOP != getch()) {
    putch((char) STK_NOSYNC);
    return;
  }
  putch((char) STK_INSYNC);
  if (memtype == 'F') result = flash_read_page(length);
  if (memtype == 'E') result = eeprom_read_page(length);
  putch(result);
  return;
}

void read_signature() {
  if (CRC_EOP != getch()) {
    putch((char) STK_NOSYNC);
    return;
  }
  putch((char) STK_INSYNC);
  uint8_t high = spi_transaction(0x30, 0x00, 0x00, 0x00);
  putch((char) high);
  uint8_t middle = spi_transaction(0x30, 0x00, 0x01, 0x00);
  putch((char) middle);
  uint8_t low = spi_transaction(0x30, 0x00, 0x02, 0x00);
  putch((char) low);
  putch((char) STK_OK);
}
//////////////////////////////////////////
//////////////////////////////////////////


////////////////////////////////////
////////////////////////////////////
int avrisp() { 
  uint8_t data, low, high;
  uint8_t ch = getch();
  switch (ch) {
  case '0': // signon
    empty_reply();
    break;
  case '1':
    if (getch() == CRC_EOP) {
      putch((char) STK_INSYNC);
      Serial1.print("AVR ISP");
      putch((char) STK_OK);
    }
    break;
  case 'A':
    get_version(getch());
    break;
  case 'B':
    readbytes(20);
    set_parameters();
    empty_reply();
    break;
  case 'E': // extended parameters - ignore for now
    readbytes(5);
    empty_reply();
    break;

  case 'P':
    start_pmode();
    empty_reply();
    break;
  case 'U':
    here = getch();
    here += 256 * getch();
    empty_reply();
    break;

  case 0x60: //STK_PROG_FLASH
    low = getch();
    high = getch();
    empty_reply();
    break;
  case 0x61: //STK_PROG_DATA
    data = getch();
    empty_reply();
    break;

  case 0x64: //STK_PROG_PAGE
    program_page();
    break;
    
  case 0x74: //STK_READ_PAGE
    read_page();    
    break;

  case 'V':
    universal();
    break;
  case 'Q':
    error=0;
    end_pmode();
    empty_reply();
    break;
    
  case 0x75: //STK_READ_SIGN
    read_signature();
    break;

  // expecting a command, not CRC_EOP
  // this is how we can get back in sync
  case CRC_EOP:
    putch((char) STK_NOSYNC);
    break;
    
  // anything else we will return STK_UNKNOWN
  default:
    if (CRC_EOP == getch()) 
      putch((char)STK_UNKNOWN);
    else
      putch((char)STK_NOSYNC);
  }
}

