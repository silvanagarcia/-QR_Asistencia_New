# Documentacion

## Bitacora

### 26/06/2024

* ¿El esp32 puede mantener encendido la placa wifi y  bluet a la ves?

El ESP32 puede utilizar Wi-Fi y Bluetooth en simultáneo. Puedes conectar a redes Wi-Fi mientras mantienes una conexión Bluetooth activa para comunicarte con otros dispositivos o periféricos. 

-----------------------------------------------------------------------------------------
es necesario para el esp32
https://dl.espressif.com/dl/package_esp32_index.json
te ayudara a instalar la libreria esp32 
###  03/07/2024
*¿que tipos de datos puedo obtener con la placa de bluetooth?

con ESP32 podemos saber si hay dispositivos bluetooth cerca, 
independientemente si están vinculados o no.

Esto se hace midiendo la intensidad de la señal (RSSI) y comprobando cual es la más fuerte. 
Aún así y ajustando un poco más el código, 
podrías incluso identificar el dispositivo mediante la dirección MAC.


### 29/07/2024
El proyecto se cambio por tomar asistencia con un codigo QR .

Utilizaremos el "Heltec WIFI KIT 32".

Estamos intentando que en la pantalla del "heltec htit-WB32" me imprima algo,
ponemos este enlace (https://dl.espressif.com/dl/package_esp32_index.json) en la preferencias y lo pegas en el gestor de URLs.
para descargar la libreria ESP32; en el gestor de la libreria tengo que buscar la "U8g2" y la instalamos 	
luego copilaremos un ejemplo de la libreria "U8x8".

un valor que tiene que estar por defecto es "U8X8_SSD1306_128X64_NONAME_SW_I2C u8x8(/* clock=/ 15, / data=/ 4, / reset=*/ 16);"
debido que la pantalla que esta soldad a en sos pines 15, 4 y 16.

usando en ejemplo "Graphics Test" con el parame que dijimos que iba a ir por defecto; el ejemplo
te mostrara todo lo que se puede imprimir en la pantalla.
### 30/07/2024
	En la practica de hoy hemo usado un ejemplo del "heltec wifi kit 32"; el ejemplo "XBM", con el cual
bamos a poder imprimir un logo en la pantalla del heltec.
en este ejemplo tendremo 
un valor que tiene que estar por defecto es "U8G2_SSD1306_128X64_NONAME_1_SW_I2C u8g2(U8G2_R0, /* clock=*/ 15, /* data=*/ 4, /* reset=*/ 16);"
debido que la pantalla que esta soldad a en sos pines 15, 4 y 16.


	Tambien hemos segido un tutoria para poder agarrar una imagen pasarla a formato BPM ,
y luego en hexadecimal, para coder cambiar el logo del ejemplo por nuestra imagen.

	La imagen tiene que ser 128x64 px y tiene que ser monocromatica blanco/negro.

	El tutorial que segimos se puede encontrar en este sitio: https://www.instructables.com/Display-Your-Photo-on-OLED-Display/
	El siguiente sitio ayudara a crea codigo QR que podriamos usar como ejercicio para mostrarlos en el heltec :https://qr.io/
	El siguiente sitio nos a ayudado con el heltec : "https://robotzero.one/heltec-wifi-kit-32/"

	Hy quedamo en que tendriamos que practiar lo que hemos echo en este dia
### 05/08/2024 
	El el tutorial del dia 30/07 nos hacer descargar un conversor de imagen y nos explica a como modificarlo para la convercion apropiada
para nosotro, pero hoy hemos descubierto que cuando mostramos la imagen que convertimos para el Heltec wifi kit 32, la misma estaba 
,por asi decirlo, al reves; los que nosotros hicimos fue que el el conversor elegimos una opcion de "invertir" en los ajuste de convercion
y haci nos dio la image en el sentido que nosotros quisimos.
	Ahora nos queda acomodar la imagen, porque a la hora de mostrarla por la pantallita tenian una inperfecciones que para 
el proyecto no es muy bueno.
### 06/08/2024
	el dian de hoy hemo probado una libreria que nos genera un QR, que se llama "QRCode"
, la complicacion que esmos tenido que que la libleria no es compatible con el esp32, pero la hemos solusionado cambinado 
el nombre de la libreria de "qrcode" a "qrcoderm" tanto el ".c" como el ".h", y dentro del ".c" en la linea 33 tambien le cambiamos el nombre
del "#include "qrcode.h"" a "#include "qrcoderm.h"".
	Y dentro del ejemplo del "QRcode", en el "#include "qrcode.h"" lo tendremos que cambiar a "#include "qrcoderm.h"" 
para que por lo menos podamos compilarlo para el esp32. Y nos faltara ver si lo hacemo funcionar.

El lugar donde encontraras para poder modificarlo en mi caso es "C:\Users\nacho\OneDrive\Documentos\Arduino\libraries\QRCode-master\src"

	Y tambien el el ".c" es necesario comentar todas las lineas que tenga "#pragma mark"

	Pero igual estubimos intentando comvertir una imagen para que me los muestre en el heltec wifi kit 32.
	
	Este link son ayudo con el QRcode: https://www.arduino.cc/reference/en/libraries/qrcode/
*	Este link es con el que descargamos el QRcode: https://github.com/ricmoo/QRCode 
¿como hago para utilizar la libreria QRcode en un "heltec wifi kit 32"?
### 07/08/2024
	hemos podido mostrar un codigo QR por una pantalla OLED con un "Esp32-nodeMCU" 
que utiliza comunicasion I2C. El QR que genera nos mostro "Hello World" porque eso es lo que nosotros 
le configuramo en el programa(programa que escribiendo "123", por ejemplo, te lo comvierte en QR para poder escanear)

	Hemos quedado en entender más sobre la libreria "QRcode" que la renombramo para el esp32y la libreria U8G2
	Link para la QRcode: https://github.com/ricmoo/QRCode 
	Link para la U8G2: https://github.com/olikraus/u8g2
La placa que usamos en le arduino es "NodeMCU-32s"
### 12/08/2024
	hemos modificado el programa de qr, antes lo que acia era mostrar un qr que al escanearlo te mostrabar "Hello World",
pero ahora le hemos puesto un contador, que a una valor "num" le ivamos sumando de a uno y te mostraba un QR, que al escanearlo 
te aparecia el valor de "num"
	por ejemplo num = 1; el QR que te mostraba, al escanearlo te iba a aparecer 1; si num le suma uno, quedaria 2 y el QR que leeras 
te debe mostrar 2, y asi susecibamente.
tubimos un problema: que el QR de ves en cuando salia fallado, pero era por un signo que no iba.

	Hemos quedado que habra un QR por cada aula, lo que esta codificado para mostrar el QR tendra un codigo de aula
mas el codigo que se genera de manera random, mas o manenos algo asi "001-434343"; el "001" significara el codigo de aula, el "434343"
el codigo de verificasion y un "-" que los separe

### 09/09/2024

	Con este link tendremos informacion sobre los pines del esp32 node32s (https://www.instructables.com/ESP32-Internal-Details-and-Pinout/)
	
	Usamos el dato que nos da un ping analogico del esp32, para la semilla de un numera random, asi nos aseguramos que le numero que se muestra 
	en el QR sea lo mas random posible
	
	link para aser get y post (https://randomnerdtutorials.com/esp32-http-get-post-arduino/#http-post)
	
### 16/09/2024
	
	luego de poder hacer un POST desde el ESP32 hacia mockoon, e prosegido con la implemantacion de hacer un codigo
	que genere un numero random, luego ese numero lo muestre en formato QR y por ultimo hacer un POST de ese numero 
	hacia mockoon.
	el numero random esta entre los parametros 000000 y 999999.
	la manera del que los envia es de formato json, de la sigiente manera:

	{
		"nombre": "ESP32", 
		"valor": "numero"
	}
	todo este proceso se hara cada 4 segundo (el tiempo sera modificado de acuerdo a cada uno)
