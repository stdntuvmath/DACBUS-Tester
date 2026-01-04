#include <Arduino.h>




// ***********DACBUS TESTER ARDUINO CODE*************
// *This program will listen for data coming from the
// *C# app and coming from the card tester and it will
// *relay the data to each respectively.
// *
// *Author: Brandon Turner @ Cymstar
// *Date: 20240509 v1.0
// *
// *This code works in tandem with 
// *
// ***********DACBUS TESTER ARDUINO CODE*************

//Global Variable Declaration

  



//Global Variable Declaration


void myLoop()
{



  //starting while loop
  
  bool ender = true;

  while(ender)
  {
    
    
    //wait for input from computer
    if(Serial.available()>0)
    {
      //starting indicator
      digitalWrite(13, HIGH);
      delay(3000);
      digitalWrite(13, LOW);
      delay(3000);

      String recievedFromComputer = Serial.readString(); 

      ender = false;

      if(recievedFromComputer.indexOf("ADDRONE") == 0)
      {

        //put string into array

        char buffer[recievedFromComputer.length()+1];//something to put the string into
        recievedFromComputer.toCharArray(buffer, recievedFromComputer.length()+1);//the buffer variable is the array
        



        for (int i = 0; i < recievedFromComputer.length()+1; i++)
        {
          //put binary number into array

          digitalWrite(13, HIGH);
          delay(3000);
          digitalWrite(13, LOW);
          delay(3000);
          
          //set mux channel and 
          if(i == 0)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, LOW);
            digitalWrite(0, LOW);//LSB
            
          }

          if(i == 1)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, LOW);
            digitalWrite(0, HIGH);//LSB
          }

          if(i == 2)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, HIGH);
            digitalWrite(0, LOW);//LSB
          }

          if(i == 3)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, HIGH);
            digitalWrite(0, HIGH);//LSB
          }

          if(i == 4)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, LOW);
            digitalWrite(0, LOW);//LSB
          }

          if(i == 5)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, LOW);
            digitalWrite(0, HIGH);//LSB
          }

          if(i == 6)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, HIGH);
            digitalWrite(0, LOW);//LSB
          }

          if(i == 7)
          {
            digitalWrite(3, LOW);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, HIGH);
            digitalWrite(0, HIGH);//LSB
          }

          if(i == 8)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, LOW);
            digitalWrite(0, LOW);//LSB
          }

          if(i == 9)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, LOW);
            digitalWrite(0, HIGH);//LSB
          }

          if(i == 10)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, HIGH);
            digitalWrite(0, LOW);//LSB
          }

          if(i == 11)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, LOW);
            digitalWrite(1, HIGH);
            digitalWrite(0, HIGH);//LSB
          }

          if(i == 12)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, LOW);
            digitalWrite(0, LOW);//LSB
          }

          if(i == 13)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, LOW);
            digitalWrite(0, HIGH);//LSB
          }

          if(i == 14)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, HIGH);
            digitalWrite(0, LOW);//LSB
          }

          if(i == 15)
          {
            digitalWrite(3, HIGH);//MSB
            digitalWrite(2, HIGH);
            digitalWrite(1, HIGH);
            digitalWrite(0, HIGH);//LSB
          }
        

        

          if(recievedFromComputer[i] == '0')
          {
            digitalWrite(4, LOW); //send signal bit from adruino to mux as integer
          }
          if(recievedFromComputer[i] == '1')
          {
            digitalWrite(4, HIGH); //send signal bit from adruino to mux as integer
          }

          
            
        } 
        
      }   
    
    }
    
  }

}



//main setup
void setup() 
{

  //Variable Declaration
  //assign muxAddressPins


  //analog pins
  pinMode(A0, INPUT);


  //digital pins
  pinMode(0, OUTPUT);
  pinMode(1, OUTPUT);
  pinMode(2, OUTPUT);
  pinMode(3, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(5, OUTPUT);
  pinMode(6, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(9, OUTPUT);
  pinMode(10, OUTPUT);
  pinMode(11, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(13, OUTPUT);
  
  //Variable Declaration

  //Begin serial clock
  Serial.begin(9600);
  //Begin serial clock

  myLoop();


}
//main setup


//--------------------------------------------------------------------------------------------------

//main loop
void loop() 
{


}
//main loop


//-----------------------------------------------------------------------------------------------------------




