

void Post(String codigo);
void Post(String codAula, String codNumero){
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(serverUrl);
    http.addHeader("Content-Type", "application/json");
    
    String postData = "{\"key\": \""+codAula+"\", \"valor\": \""+codNumero+"\"}";
    
    int httpResponseCode = http.POST(postData);
    
    if (httpResponseCode > 0) {
      Serial.println("-------------------------");
      Serial.println(httpResponseCode);
      String response = http.getString();
      Serial.println("Respuesta del servidor:");
      Serial.println(response);
      Serial.println("");
    } else {
      Serial.print("Error en la solicitud POST. Código de error: ");
      Serial.println(httpResponseCode);
    }
    http.end();
  }

}
