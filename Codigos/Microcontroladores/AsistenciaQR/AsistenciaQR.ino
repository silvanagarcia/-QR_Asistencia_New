#include "Variables.h"
#include "GeneratorQR.h"
#include "HacerPOST.h"
#include "NumeroRandom.h"
bool siConect();
void conect();

void setup() {
  
  
  Serial.begin(115200);
  conect();
  
  u8g2.begin();
    
 
  randomSeed(analogRead(pinEsp));

   

  Serial.println();
  Serial.println("Conectado a WiFi");
 //display.begin(SSD1306_EXTERNALVCC, 0x3c);

  //showQRCode("https://cyberello.com");
  

}

void loop() {
  mostrar = codificacion();
  showQRCode(mostrar);
  
  Serial.println(mostrar);
  Serial.println("-------------------------");

  for(int i=0;i<10;i++){
    delay(1000);
    Serial.print(i);  
  }
  
  //Serial.println("hola Mundo!!");
}

void conect(){
  WiFi.begin(ssidL, passwordL); 
  if (siConect()){
    Serial.print("Se logro conectar a: ");
    Serial.println(ssidL);
  }else{
    WiFi.begin(ssidC, passwordC); 
    if (siConect()){
      Serial.print("Se logro conectar a:");
      Serial.println(ssidC);
    }else{
      Serial.println("no se puedo conectar a niguna red");
    }
  }
  Serial.println("Esta es la url que se usara");
  Serial.println(serverUrl);
}

bool siConect(){
  int intentos=0;
  while(WiFi.status() != WL_CONNECTED && intentos < 5){
    delay(1000);  
    Serial.print(".");
    intentos++;
  }
  return WiFi.status() == WL_CONNECTED;
}
