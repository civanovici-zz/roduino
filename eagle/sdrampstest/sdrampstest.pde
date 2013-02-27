
const int chipSelect = 53;    
#include <SD.h>

Sd2Card card;
SdVolume volume;
SdFile root;

void setup() {
  
  Serial.begin(115200);
  Serial.print("\nInitializing SD card...");
  
  pinMode(53, OUTPUT);
  pinMode(48, OUTPUT);  
  digitalWrite(48,HIGH);
  pinMode(SPI_MISO_PIN, INPUT);
  pinMode(SPI_MOSI_PIN, OUTPUT);
  pinMode(SPI_SCK_PIN, OUTPUT);
  
  digitalWrite(SPI_MOSI_PIN,LOW);
  digitalWrite(SPI_SCK_PIN,LOW);
  digitalWrite(53,LOW);
  delay(1);
  
  SD.begin(53);
}


void loop () {
 
 
   if (!card.init(SPI_FULL_SPEED, chipSelect)) {
    Serial.println("initialization failed. Things to check:");
    Serial.println("* is a card is inserted?");
    Serial.println("* Is your wiring correct?");
    Serial.println("* did you change the chipSelect pin to match your shield or module?");
    return;
  } else {
   Serial.println("Wiring is correct and a card is present.");
  }

  // print the type of card
  Serial.print("\nCard type: ");
  switch(card.type()) {
    case SD_CARD_TYPE_SD1:
      Serial.println("SD1");
      break;
    case SD_CARD_TYPE_SD2:
      Serial.println("SD2");
      break;
    case SD_CARD_TYPE_SDHC:
      Serial.println("SDHC");
      break;
    default:
      Serial.println("Unknown");
  }

  // Now we will try to open the 'volume'/'partition' - it should be FAT16 or FAT32
  if (!volume.init(card)) {
    Serial.println("Could not find FAT16/FAT32 partition.\nMake sure you've formatted the card");
    return;
  }


  // print the type and size of the first FAT-type volume
  long volumesize;
  Serial.print("\nVolume type is FAT");
  Serial.println(volume.fatType(), DEC);
  Serial.println();
 
  volumesize = volume.blocksPerCluster();    // clusters are collections of blocks
  volumesize *= volume.clusterCount();       // we'll have a lot of clusters
  volumesize *= 512;                            // SD card blocks are always 512 bytes
  Serial.print("Volume size (bytes): ");
  Serial.println(volumesize);
  Serial.print("Volume size (Kbytes): ");
  volumesize /= 1024;
  Serial.println(volumesize);
  Serial.print("Volume size (Mbytes): ");
  volumesize /= 1024;
  Serial.println(volumesize);

 
  Serial.println("\nFiles found on the card (name, date and size in bytes): ");
  root.openRoot(volume);
 
  // list all files in the card with date and size
  root.ls(LS_R | LS_DATE | LS_SIZE);
  delay (3000);
}
