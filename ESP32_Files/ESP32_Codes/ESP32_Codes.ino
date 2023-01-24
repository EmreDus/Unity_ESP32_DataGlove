#include "I2Cdev.h"
#include "MPU6050_6Axis_MotionApps20.h"
#include "MPU6050.h"
#include "Wire.h"


MPU6050 mpu;

#define OUTPUT_READABLE_QUATERNION

#define INTERRUPT_PIN 2
#define LED_PIN 12
bool blinkState = false;

// MPU control/status vars
bool dmpReady = false;   // set true if DMP init was successful
uint8_t mpuIntStatus;    // holds actual interrupt status byte from MPU
uint8_t devStatus;       // return status after each device operation (0 = success, !0 = error)
uint16_t packetSize;     // expected DMP packet size (default is 42 bytes)
uint16_t fifoCount;      // count of all bytes currently in FIFO
uint8_t fifoBuffer[64];  // FIFO storage buffer

// orientation/motion vars
Quaternion q;         // [w, x, y, z]         quaternion container
VectorInt16 aa;       // [x, y, z]            accel sensor measurements
VectorInt16 aaReal;   // [x, y, z]            gravity-free accel sensor measurements
VectorInt16 aaWorld;  // [x, y, z]            world-frame accel sensor measurements
VectorFloat gravity;  // [x, y, z]            gravity vector
float euler[3];       // [psi, theta, phi]    Euler angle container
float ypr[3];         // [yaw, pitch, roll]   yaw/pitch/roll container and gravity vector

uint8_t teapotPacket[14] = { '$', 0x02, 0, 0, 0, 0, 0, 0, 0, 0, 0x00, 0x00, '\r', '\n' };


// ***********INTERRUPT*********
volatile bool mpuInterrupt = false;  // indicates whether MPU interrupt pin has gone high
void dmpDataReady() {
  mpuInterrupt = true;
}


float RateRoll, RatePitch, RateYaw;
float AccX, AccY, AccZ;
float AngleRoll, AnglePitch;
float LoopTimer;

int Min_isaret, Min_orta, Min_yuzuk, Min_serce, Min_bas;
int Max_isaret = 50, Max_orta = 50, Max_yuzuk = 50, Max_serce = 80, Max_bas = 50;

void setup() {
  Wire.begin();
  Serial.begin(9600);

  mpu.initialize();
  pinMode(INTERRUPT_PIN, INPUT);

  //****DMP****
  devStatus = mpu.dmpInitialize();

  mpu.setXGyroOffset(108);
  mpu.setYGyroOffset(36);
  mpu.setZGyroOffset(48);
  mpu.setXAccelOffset(-1585);
  mpu.setYAccelOffset(3611);
  mpu.setZAccelOffset(765);


  // make sure it worked (returns 0 if so)
  if (devStatus == 0) {
    // Calibration Time: generate offsets and calibrate our MPU6050
    mpu.CalibrateAccel(6);
    mpu.CalibrateGyro(6);
    mpu.PrintActiveOffsets();
    // turn on the DMP, now that it's ready
    Serial.println(F("Enabling DMP..."));
    mpu.setDMPEnabled(true);

    // enable Arduino interrupt detection
    Serial.print(F("Enabling interrupt detection (Arduino external interrupt "));
    Serial.print(digitalPinToInterrupt(INTERRUPT_PIN));
    Serial.println(F(")..."));
    attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), dmpDataReady, RISING);
    mpuIntStatus = mpu.getIntStatus();

    // set our DMP Ready flag so the main loop() function knows it's okay to use it
    Serial.println(F("DMP ready! Waiting for first interrupt..."));
    dmpReady = true;

    // get expected DMP packet size for later comparison
    packetSize = mpu.dmpGetFIFOPacketSize();
  } else {
    // ERROR!
    // 1 = initial memory load failed
    // 2 = DMP configuration updates failed
    // (if it's going to break, usually the code will be 1)
    Serial.print(F("DMP Initialization failed (code "));
    Serial.print(devStatus);
    Serial.println(F(")"));
  }

  //PIN Configure
  pinMode(LED_PIN, OUTPUT);
  pinMode(34, INPUT);
  pinMode(35, INPUT);
  pinMode(32, INPUT);
  pinMode(33, INPUT);
  pinMode(25, INPUT);

  String inputString = "";
  bool stringComplete = false;


}

void loop() {

  int serce = analogRead(34);
  int yuzuk = analogRead(35);
  int orta = analogRead(32);
  int isaret = analogRead(33);
  int bas = analogRead(25);

  /*String inputString = "";
  bool stringComplete = false;
  
  if (Serial.available()) {
    char inChar = (char)Serial.read();
    inputString += inChar;
    if (inChar == '\n') {
      stringComplete = true;
    }
  }
  if (stringComplete) {
    // Split the inputString by "," and store the values in different variables
    int index = 0;
    char *cstr = new char[inputString.length() + 1];
    strcpy(cstr, inputString.c_str());
    char *p = strtok(cstr, ",");
    while (p != NULL) {
      if(index==0) Min_isaret=atoi(p);
      else if(index==1) Min_orta=atoi(p);
      else if(index==2) Min_yuzuk=atoi(p);
      else if(index==3) Min_serce=atoi(p);
      else if(index==4) Min_bas=atoi(p);
      else if(index==5) Max_isaret=atoi(p);
      else if(index==6) Max_orta=atoi(p);
      else if(index==7) Max_yuzuk=atoi(p);
      else if(index==8) Max_serce=atoi(p);
      else if(index==9) Max_bas=atoi(p);
      index++;
      p = strtok(NULL, ",");
    }
    // Now you can use the different variables for example:
    // Min_isaret, Min_orta, Min_yuzuk, Min_serce, Min_bas
    // Max_isaret, Max_orta, Max_yuzuk, Max_serce, Max_bas
    // Clear the inputString for the next incoming data
    delete[] cstr;
    inputString = "";
    stringComplete = false;
  }*/
  
    serce = map(serce, 0, 4095, Max_serce, Min_serce);
    yuzuk = map(yuzuk, 0, 4095, Max_yuzuk, Min_yuzuk);
    orta = map(orta, 0, 4095, Max_orta, Min_orta);
    isaret = map(isaret, 0, 4095, Max_isaret, Min_isaret);
    bas = map(bas, 0, 4095, Max_bas, Min_bas);


  if (mpu.dmpGetCurrentFIFOPacket(fifoBuffer)) {
    // Quaternions
    mpu.dmpGetQuaternion(&q, fifoBuffer);
    Serial.print(q.w);
    Serial.print(",");
    Serial.print(q.x);
    Serial.print(",");
    Serial.print(q.y);
    Serial.print(",");
    Serial.print(q.z);
    Serial.print(",");
  }

  // POTANS DEĞERLERİ
  Serial.print(serce);
  Serial.print(",");
  Serial.print(yuzuk);
  Serial.print(",");
  Serial.print(orta);
  Serial.print(",");
  Serial.print(isaret);
  Serial.print(",");
  Serial.println(bas);
  //Serial.print(",");


  delay(50);  //for unity
}