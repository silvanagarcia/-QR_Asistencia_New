
#include <Wire.h>
#include <qrcoderm.h>
#include <Arduino.h>
#include <WiFi.h>
#include <HTTPClient.h>

#include <U8g2lib.h>

#define SCREEN_WIDTH 128  
#define SCREEN_HEIGHT 64
#define OLED_RESET -1


const char* ssidL = "laboratorio";
const char* ssidC = "Casa Guerra";

const char* passwordL = "laboratorio";
const char* passwordC = "Nachoytom";
//77.81.230.76:5095
const char* serverUrl = "http://77.81.230.76:5095/api/QR";
//const char* serverUrl = "http://192.168.0.104:3002/api/QR";

//Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);
U8G2_SSD1306_128X64_NONAME_F_HW_I2C u8g2(U8G2_R0, /* reset=*/ U8X8_PIN_NONE);

const int pinEsp = 35;

String id_aula = "001";
String mostrar = "";
long valor = 0; 
