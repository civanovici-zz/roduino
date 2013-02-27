#define SAMPLE_RATE 44 //[2, 4, 7, 8, 14, 22, 29, (44), 88]

#define ADCHPD 1//[(0), 1]
//- this either turns on or off the digital highpass filter on the 
//codec input.  this filter ensures that the signal you are sampling 
//is centered in the range.  if you want to have a DC control voltage 
//as in input to the codec, you will need to turn this off by setting ADCHPD 1.  
//for all other signals, its best to leave it enabled, as its well below the audible 
//range and will not effect the sound quality.

#define LINVOL 23// [0 -> 31, (23)]
#define RINVOL 23// [0 -> 31, (23)]
//- this sets the line-in volume, in 1.5dB steps from -34.5dB to +12dB. 
//the default setting is 0dB.

#define LHPVOL 100// [0, 48 -> 127, (121)]
#define RHPVOL  100 //[0, 48 -> 127, (121)]
//- this sets the headphone volume output in 1dB steps from -73dB to +6dB.  
//the default setting is 0dB. LHPVOL 0 mutes the output, as does any value 
//from 0 -> 47, but for simplicity's sake, mute is given as 0 with the other values not defined.

#define MICBOOST 1// [(0), 1]
//- this either enables or disables a fixed volume boost on the microphone input. 
//MICBOOST 0 disables the boost, and MICBOOST 1 gives a +10dB volume increase.

#define MUTEMIC 0// [0, (1)]
//- this eithe enables or disables the microphone input to the ADC.  
//MUTEMIC 1 mutes the signal to the ADC.

#define INSEL 0// [(0), 1]
//- this selects wether the microphone or line-in inputs go the ADC. 
//INSEL 0 selects the line-in inputs, and INSEL 1 selects the microphone input.

#define BYPASS 0//[(0), 1]
//- this either enables or disables a pass-through from line-in to line-out, 
//without any digitization.  BYPASS 0 disables the pass-through.  this only applies to the line-in inputs.

#define DACSEL 1// [0, (1)]
//- this either enables or disables the DAC from presenting its signal at the output.  
//DACSEL 1 enables the DAC output.

#define SIDETONE 1// [(0), 1]
//- this either enables or disables the sidetone signal at the output. 
//the sidetone is a reduced version of the microphone input signal that can be presented 
//at the output. it is often used in telephony applications to allow the speaker to here what she is saying. SIDETONE 0 disables the sidetone.  this only applies to the microphone input.

#define SIDEATT 0//[(0) -> 3]
//- this sets the level of the sidetone signal that appears at the output.  
//the level varies from -6dB at SIDEATT 0 to -15dB at SIDEATT 3, in -3dB steps.

#define ADCS 0// [0, 1, (2), 3, 4]
//- this sets the number of ADCs to be sampled by the microcontroller.  
//you should always set this to the minimum number of ADCs you intend to use, 
//as sampling the ADCs takes up precious processing time.

#define HYST 128// [0 -> 255, (32)]
//- this sets the deadband hysteresis on the adc samples.  
//this eliminates jitter in yor adc sample value due to random noise.  
//it takes up a bit more processing time, so if you do not need it, 
//you can set HYST 0 to turn it off.

#define OVERSAMPLE 32//[1, 2, 4, 8, 16, 32, (64)]
//- this sets the amount of time the ADC samples the MOD pot before it presents 
//a new number.  The larger the OVERSAMPLE number, the less noise there will be in the
//data, but the less fast the MOD pot will respond.  64 is a bit overkill, 
//but this gives really clean 16b numbers.  
//if you drop it down to 1, then you get clean 9b or 10b numbers.

// include necessary libraries
#include <Wire.h>
#include <SPI.h>
#include <AudioCodec.h>

// create data variables for audio transfer
int left_in = 0x0000;
int left_out = 0x0000;
int right_in = 0x0000;
int right_out = 0x0000;


void setup() {
  AudioCodec_init(); // setup codec registers
  // call this last if setting up other parts
}

void loop() {
  //while (1); // reduces clock jitter
}

// timer1 interrupt routine - all data processed here
ISR(TIMER1_COMPA_vect, ISR_NAKED) { // dont store any registers

  // &'s are necessary on data_in variables
  AudioCodec_data(&left_in, &right_in, left_out, right_out);
  
  // pass data through
  left_out = left_in;
  right_out = right_in;
  
  // dont forget to return from interrupt
  reti();
}
