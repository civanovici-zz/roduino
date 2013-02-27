#define stepPin 2
#define dirPin 3
#define enablePin 4
void setup()
{
// We set the enable pin to be an output
pinMode(enablePin, OUTPUT);
// then we set it HIGH so that the board is disabled until we
// get into a known state.
digitalWrite(enablePin, HIGH);
Serial.begin(9600);
Serial.println("Starting stepper exerciser.");
pinMode(stepPin, OUTPUT);
pinMode(dirPin, OUTPUT);
}
void loop()
{
int j;
// set the enablePin low so that we can now use our stepper driver.
digitalWrite(enablePin, LOW);
// wait a few microseconds for the enable to take effect
// (That isn't in the spec sheet I just added it for sanity.)
delayMicroseconds(2);
// we set the direction pin in an arbitrary direction.
digitalWrite(dirPin, HIGH);
for(j=0; j<=10000; j++) {
digitalWrite(stepPin, LOW);
delayMicroseconds(2);
digitalWrite(stepPin, HIGH);
delayMicroseconds(1000);
}}
