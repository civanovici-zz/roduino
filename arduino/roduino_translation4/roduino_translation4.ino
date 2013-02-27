#include <mcp4241.h>

#define SS   10  // Slave Select
#define SCK  13  // Shift CLK
#define MOSI 11  // Master Out Slave In
#define MISO 12  // Master In Slave Out
C_mcp4241Pot c(SS,SCK,MOSI,MISO);
volatile int value = 0;


//channels buttons {0,1,3,6} // arduino A0,A1
//volum buttons {A5,A6} //arduino A5,A4
const int btn_channels[6]={0,1,3,6,A5,A6}; 
int lastButtonState[6]={LOW,LOW,LOW,LOW,LOW,LOW};
int buttonState[6]={LOW,LOW,LOW,LOW,LOW,LOW};
long lastDebounceTime[6]={0,0,0,0,0,0};

const int led_pins[3]={2,4,5}; //led pins
const int sw_pins[3]={7,8,9};//SW2,SW1,SW0 

long debounceDelay=50; //delay to check if button is pressed
int currentChannell; // the current channell

void setup(){
//  Serial.begin(9600);
  delay(100);
  c.Begin();
  initPins();
  channelSelected(1);
  channelSelected(0); //set default channel 0
}

void loop(){
  watchChannels();
}

void initPins(){
  for(int i=0;i<6;i++){ //set input for channels listners
    pinMode(btn_channels[i],OUTPUT);
  }
  for(int i=0;i<3;i++){ //set led pins    
    digitalWrite(led_pins[i],LOW);
    pinMode(led_pins[i],OUTPUT);
    pinMode(led_pins[i],INPUT);
  }
  
  for(int i=0;i<3;i++){ //set audio command pins
    pinMode(sw_pins[i],OUTPUT);
  }
}

void watchChannels(){
  for(int i=0;i<6;i++)
  {
    int reading=digitalRead(btn_channels[i]);
    
    if(reading!=lastButtonState[i] ){      
      lastDebounceTime[i] = millis();
    }
    if ((millis() - lastDebounceTime[i]) > debounceDelay) {
      buttonState[i] = reading;
      if(buttonState[i]==HIGH){
        channelSelected(i);        
      }
    }
    
    lastButtonState[i] = reading;
  }
}
byte b = 0x00u;
byte val = 0x00u;

//void debugVol(byte b, byte val,boolean up){
//  Serial.print("Wiper1 level: ");
//  Serial.print(b,HEX);
//  Serial.print(" Hex val wiper1:");
//  Serial.print(val,HEX);
//  if(up)
//    Serial.print(" Incremented\n"); 
//  else
//    Serial.print(" Decrement\n"); 
//}

void channelSelected(int i){
  if(i==4){
    c.Inc_Level_Wiper1();
    b=(byte)c.Get_Level_Wiper1();
    val = c.Get_Value_Wiper1();
//    debugVol(b,val,true);
  }
  if(i==5){
    c.Dec_Level_Wiper1();
    b=(byte)c.Get_Level_Wiper1();
    val = c.Get_Value_Wiper1();
//    debugVol(b,val,false);
  }
  if(currentChannell!=i){
    switch(i){
      case 0:
        pinMode(led_pins[0],OUTPUT);
        pinMode(led_pins[1],OUTPUT);
        pinMode(led_pins[2],INPUT);
        digitalWrite(led_pins[0],HIGH);        
        digitalWrite(led_pins[1],LOW);
        digitalWrite(sw_pins[0],LOW);
        digitalWrite(sw_pins[1],LOW);
        digitalWrite(sw_pins[2],LOW);
        break;
      case 1:
        pinMode(led_pins[0],OUTPUT);
        pinMode(led_pins[1],OUTPUT);
        pinMode(led_pins[2],INPUT);
        digitalWrite(led_pins[0],LOW);        
        digitalWrite(led_pins[1],HIGH);
        digitalWrite(sw_pins[0],HIGH);
        digitalWrite(sw_pins[1],HIGH);
        digitalWrite(sw_pins[2],LOW);
        break;
      case 2:
        pinMode(led_pins[0],INPUT);
        pinMode(led_pins[1],OUTPUT);
        pinMode(led_pins[2],OUTPUT);
        digitalWrite(led_pins[1],LOW);        
        digitalWrite(led_pins[2],HIGH);
        digitalWrite(sw_pins[0],HIGH);
        digitalWrite(sw_pins[1],LOW);
        digitalWrite(sw_pins[2],HIGH);
        break;
      case 3:
        pinMode(led_pins[0],INPUT);
        pinMode(led_pins[1],OUTPUT);
        pinMode(led_pins[2],OUTPUT);
        digitalWrite(led_pins[1],HIGH);        
        digitalWrite(led_pins[2],LOW);
        digitalWrite(sw_pins[0],HIGH);
        digitalWrite(sw_pins[1],LOW);
        digitalWrite(sw_pins[2],LOW);
        break;
    }
    currentChannell=i;
//    Serial.print("change channel: ");
//    Serial.println(i);
  }
}


