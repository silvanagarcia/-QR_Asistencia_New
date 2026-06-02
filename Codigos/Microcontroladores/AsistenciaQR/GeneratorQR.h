const int qrCodeVersion = 3;
const int pixelSize = 2;

void showQRCode(String qrCodeString);
void showQRCode(String qrCodeString) {

  QRCode qrcode;

  uint8_t qrcodeBytes[qrcode_getBufferSize(qrCodeVersion)];
  qrcode_initText(&qrcode, qrcodeBytes, qrCodeVersion, ECC_LOW,
                  qrCodeString.c_str());

  //display.clearDisplay();

  int startX = (SCREEN_WIDTH - (qrcode.size * pixelSize) - (pixelSize * 2))
               / 2;
  int startY = (SCREEN_HEIGHT - (qrcode.size * pixelSize) - (pixelSize * 2))
               / 2;

  // startX=0;
  // startY=0;

/*
Serial.print("X: ");
Serial.println(startX);
Serial.print("Y: ");
Serial.println(startY);
  */
  int qrCodeSize = qrcode.size;
//Serial.print("qrCodeSize: ");
//Serial.println(qrCodeSize);

  u8g2.setDrawColor(1);
  u8g2.clearBuffer();
  u8g2.drawRBox(startX, startY, (qrCodeSize * pixelSize) + (pixelSize * 2),(qrCodeSize * pixelSize) + (pixelSize * 2),1);
  u8g2.sendBuffer();
   
 // display.fillRect(startX, startY, (qrCodeSize * pixelSize) + (pixelSize * 2),
 //                  (qrCodeSize * pixelSize) + (pixelSize * 2), WHITE);


   
  u8g2.setDrawColor(0);
  
  for (uint8_t y = 0; y < qrCodeSize; y++) 
  {
    for (uint8_t x = 0; x < qrCodeSize; x++) {
      if (qrcode_getModule(&qrcode, x, y))
      {
        
        u8g2.drawRBox(x * pixelSize + startX + pixelSize,y * pixelSize + startY + pixelSize, pixelSize,
                         pixelSize,1);
        
        /*display.fillRect(x * pixelSize + startX + pixelSize,
                         y * pixelSize + startY + pixelSize, pixelSize,
                         pixelSize, BLACK);
        */
      }
      else 
      {
        //Serial.print("-,");
        
        }
      
    }
    //Serial.println();
  }
  u8g2.sendBuffer();
  
  //display.display();
    
}
