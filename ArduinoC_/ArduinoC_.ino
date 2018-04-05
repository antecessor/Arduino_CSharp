
bool flag=false;
const int analogInPinl = A0; 
const int analogInPinm = A1; 
const int analogInPinr = A2;
const int ledPinl =  8; 
const int ledPinm =  9; 
const int ledPinr =  10; 

int sensorValuel = 0; 
int sensorValuem = 0; 
int sensorValuer = 0; 

String inData;
String zarbe;
String takhir;
String direc;




void setup() {
  // initialize serial communications at 9600 bps:
  Serial.begin(9600);

  pinMode(ledPinl, OUTPUT);
  pinMode(ledPinm, OUTPUT);
  pinMode(ledPinr, OUTPUT);
  digitalWrite(ledPinl, LOW);
      digitalWrite(ledPinr, LOW);
      digitalWrite(ledPinm, LOW);
}

void loop() {


   
    // read the analog in value:
  sensorValuel = analogRead(analogInPinl);
  sensorValuem = analogRead(analogInPinm);
  sensorValuer = analogRead(analogInPinr);
  // map it to the range of the analog out:
  sensorValuel = map(sensorValuel, 0, 1023, 0, 255);
  sensorValuem = map(sensorValuem, 0, 1023, 0, 255);
  sensorValuer = map(sensorValuer, 0, 1023, 0, 255);

   
  Serial.print("sensor1 = " );
  Serial.print(sensorValuel);
  Serial.print("\t sensorm =  ");
  Serial.print(sensorValuem);
  Serial.print("\t sensorr =  ");
  Serial.println(sensorValuer);
  
 
  
       
    delay(2);  
  
 
}



void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read(); 
    
    if (inChar=='r'){ 
      delay(30);
      digitalWrite(ledPinr, HIGH);
      digitalWrite(ledPinl, LOW);
      digitalWrite(ledPinm, LOW);
     
      }
      else if(inChar=='l'){
         delay(30);
        digitalWrite(ledPinl, HIGH);
      digitalWrite(ledPinr, LOW);
      digitalWrite(ledPinm, LOW);
      }
      else if(inChar=='m'){
         delay(30);
        digitalWrite(ledPinm, HIGH);
      digitalWrite(ledPinl, LOW);
      digitalWrite(ledPinr, LOW);
      }
      else if(inChar=='n'){
        digitalWrite(ledPinm, LOW);
      digitalWrite(ledPinl, LOW);
      digitalWrite(ledPinr, LOW);
      }
    
    
    
    
 } 
}

