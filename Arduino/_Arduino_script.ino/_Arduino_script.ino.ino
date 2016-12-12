
  int sentVal0 = 0;
  int sentVal1 = 0;
  int sentVal2 = 0;
  int sentVal3 = 0;
  int sentVal4 = 0;
  
// the setup routine runs once when you press reset:
void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);
}

// the loop routine runs over and over again forever:
void loop() {
  
  int sensorValue0 = analogRead(A0);
  if(sensorValue0-sentVal0 >= 2 || sensorValue0-sentVal0 <= -2){
    sentVal0 = sensorValue0;
  }
  
  int sensorValue1 = analogRead(A1);
    if(sensorValue1-sentVal1 >= 2 || sensorValue1-sentVal1 <= -2){
    sentVal1 = sensorValue1;
  }

  int sensorValue2 = analogRead(A2);
    if(sensorValue2-sentVal2 >= 2 || sensorValue2-sentVal2 <= -2){
    sentVal2 = sensorValue2;
  }

  int sensorValue3 = analogRead(A3);
    if(sensorValue3-sentVal3 >= 2 || sensorValue3-sentVal3 <= -2){
    sentVal3 = sensorValue3;
  }
  
  int sensorValue4 = analogRead(A4);
    if(sensorValue4-sentVal4 >= 2 || sensorValue4-sentVal4 <= -2){
    sentVal4 = sensorValue4;
  }

  Serial.println(map(sentVal0, 0, 1023, 1, 1000));
  Serial.println(map(sentVal1, 0, 1023, 1, 1000));
  Serial.println(map(sentVal2, 0, 1023, 1, 1000));
  Serial.println(map(sentVal3, 0, 1023, 1, 1000));
  Serial.println(map(sentVal4, 0, 1023, 0, 360));

  delay(20);        // delay in between reads for stability
}
