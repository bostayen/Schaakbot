#include <AccelStepper.h>
#include <MultiStepper.h>

//DefineI/O pins
#define PinXZero 3
#define PinYZero 4
#define PinMagneet 9

//Settings for the steppers
long MaxPosition = 10000;
long MaxSpeed = 1000;
long MaxAccel = 5000;

//Define the steppers
AccelStepper stepperX(1, 7, 8);
AccelStepper stepperY(1, 6, 5);

//Define parameters and input strings
String input;
String Input;
long x = 0;
long y = 0;

void setup()
{
  //Start serial connection
  Serial.setTimeout(50);
  Serial.begin(115200);
  //Serial.println("MoveTo;Magnet;X;Y");
  //Serial.println("Set;Accel;Speed");

  //Setup the i/O pins
  pinMode(PinXZero, INPUT_PULLUP);
  pinMode(PinYZero, INPUT_PULLUP);
  pinMode(PinMagneet, OUTPUT);
  digitalWrite(PinMagneet, LOW);

  //Setup the steppers
  stepperX.setMaxSpeed(MaxSpeed);
  stepperX.setAcceleration(MaxAccel);
  //stepperX.setPinsInverted (true, false, false);
  stepperY.setMaxSpeed(MaxSpeed);
  stepperY.setAcceleration(MaxAccel);
  stepperY.setPinsInverted (true, false, false);
}

String getValue(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = {0, -1};
  int maxIndex = data.length() - 1;

  for (int i = 0; i <= maxIndex && found <= index; i++) {
    if (data.charAt(i) == separator || i == maxIndex) {
      found++;
      strIndex[0] = strIndex[1] + 1;
      strIndex[1] = (i == maxIndex) ? i + 1 : i;
    }
  }

  return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}

void loop() {
  //Wait for serial input
  if (Serial.available() > 0) {
    Input = Serial.readString();
    //MoveTo command
    if (getValue(Input, ';', 0) == "MoveTo") {

      //Get magnet status
      if (getValue(Input, ';', 1) == "1") {
        digitalWrite(PinMagneet, HIGH);
        delay(500);
      }
      else {
        digitalWrite(PinMagneet, LOW);
        //If the magnet is of this means the system can run at full speed
        stepperX.setMaxSpeed(30000);
        stepperX.setAcceleration(30000);
        stepperY.setMaxSpeed(30000);
        stepperY.setAcceleration(30000);
      }

      //Get positions
      String X = getValue(Input, ';', 2);
      String Y = getValue(Input, ';', 3);
      x = X.toInt();
      y = Y.toInt();

      //Write positions
      if (y  <= MaxPosition) {
        stepperX.moveTo(x);
        stepperY.moveTo(y);
      }
      else {
        Serial.println("To far");
      }

      //Block till done
      while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
        stepperX.run();
        stepperY.run();
      }

      //Set speed again
      stepperX.setMaxSpeed(MaxSpeed);
      stepperX.setAcceleration(MaxAccel);
      stepperY.setMaxSpeed(MaxSpeed);
      stepperY.setAcceleration(MaxAccel);
      digitalWrite(PinMagneet, LOW);
      Serial.println("Done");
      /*
        Serial.print("Magnet: ");
        Serial.println(Magnet);
        Serial.print("X: ");
        Serial.println(x);
        Serial.print("Y: ");
        Serial.println(y);
      */
    }
    else if (getValue(Input, ';', 0) == "Set") {
      String Accel = getValue(Input, ';', 1);
      String Speed = getValue(Input, ';', 2);
      String Pos = getValue(Input, ';', 3);
      MaxPosition = Pos.toInt();
      MaxSpeed = Speed.toInt();
      MaxAccel = Accel.toInt();
      stepperX.setMaxSpeed(MaxSpeed);
      stepperX.setAcceleration(MaxAccel);
      stepperY.setMaxSpeed(MaxSpeed);
      stepperY.setAcceleration(MaxAccel);
      Serial.println("Done");
    }
    else if (getValue(Input, ';', 0) == "Zero") {
      Serial.println("Zero state");
      stepperX.setAcceleration(300000.0);
      stepperY.setAcceleration(300000.0);
      stepperX.setMaxSpeed(1000.0);
      stepperY.setMaxSpeed(1000.0);
      while (digitalRead(PinXZero) == HIGH || digitalRead(PinYZero) == HIGH) {
        if (digitalRead(PinXZero) == HIGH) {
          stepperX.move(-10);
          stepperX.runSpeed();
        }
        if (digitalRead(PinYZero) == HIGH) {
          stepperY.move(-10);
          stepperY.runSpeed();
        }
        //stepperX.runSpeed();
        //stepperY.runSpeed();
      }
      stepperX.setMaxSpeed(200);
      stepperY.setMaxSpeed(200);
      stepperX.move(200);
      stepperY.move(200);
      while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
        stepperX.run();
        stepperY.run();
      }
      while (digitalRead(PinXZero) == HIGH || digitalRead(PinYZero) == HIGH) {
        if (digitalRead(PinXZero) == HIGH) {
          stepperX.move(-1);
          stepperX.runSpeed();
        }
        if (digitalRead(PinYZero) == HIGH) {
          stepperY.move(-1);
          stepperY.runSpeed();
        }
        //stepperX.runSpeed();
        //stepperY.runSpeed();
      }
      stepperX.setAcceleration(MaxAccel);
      stepperY.setAcceleration(MaxAccel);
      stepperX.setMaxSpeed(MaxSpeed);
      stepperY.setMaxSpeed(MaxSpeed);
      stepperX.setCurrentPosition(0);
      stepperY.setCurrentPosition(0);
      /*
        stepperX.moveTo(500);
        stepperY.moveTo(500);
        while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
          stepperX.run();
          stepperY.run();
        }
        stepperX.setCurrentPosition(0);
        stepperY.setCurrentPosition(0);
      */
      Serial.println("Done");
    }
    /*
      else if (getValue(Input, ';', 0) == "What?") {
      Serial.print("Acceleratie: ");
      Serial.println(MaxAccel);
      Serial.print("Speed: ");
      Serial.println(MaxSpeed);
      Serial.print("Maxpos: ");
      Serial.println(MaxPosition);
      }
    */
    else if (getValue(Input, ';', 0) == "Demo") {
      digitalWrite(PinMagneet, HIGH);
      stepperX.moveTo(0);
      stepperY.moveTo(5000);
      while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
        stepperX.run();
        stepperY.run();
      }
      stepperX.moveTo(5000);
      stepperY.moveTo(10000);
      while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
        stepperX.run();
        stepperY.run();
      }
      stepperX.moveTo(10000);
      stepperY.moveTo(5000);
      while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
        stepperX.run();
        stepperY.run();
      }
      stepperX.moveTo(5000);
      stepperY.moveTo(0);
      while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
        stepperX.run();
        stepperY.run();
        if (Serial.available() > 0) {
          input = Serial.readString();
        }
      }
      digitalWrite(PinMagneet, LOW);
      stepperX.moveTo(0);
      stepperY.moveTo(0);
      while (stepperX.isRunning() == true || stepperY.isRunning() == true) {
        stepperX.run();
        stepperY.run();
      }
    }
    else if (getValue(Input, ';', 0) == "Test magneet") {
      digitalWrite(PinMagneet, HIGH);
      delay(1000);
      digitalWrite(PinMagneet, LOW);
    }
    else {
      Serial.println("Nope");
    }
  }
}
