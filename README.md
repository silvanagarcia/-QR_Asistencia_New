# Sistema de Asistencia QR — ITES

Sistema completo para el registro de asistencia mediante códigos QR. El alumno escanea un QR generado por un microcontrolador (ESP32), la app móvil envía el registro a una Web API, que lo guarda en una base de datos PostgreSQL.

---

## Arquitectura general

```
┌─────────────────┐        HTTP POST         ┌─────────────────────┐
│  App Móvil MAUI │ ──────────────────────▶  │   Web API (.NET 8)  │
│  (Android)      │ ◀──────────────────────  │   Asistencia.API    │
└─────────────────┘        JSON              └──────────┬──────────┘
                                                        │ EF Core
                                                        ▼
┌─────────────────┐     Genera QR            ┌─────────────────────┐
│  Microcontrol.  │ ──────────────────────▶  │   PostgreSQL        │
│  (ESP32)        │    pantalla OLED          │   QR_Asistencias    │
└─────────────────┘                          └─────────────────────┘
```

---

## Estructura del repositorio

```
QR_Asistencia/
├── Codigos/
│   ├── MAUI/                   ← App móvil (.NET MAUI, C#) ← NUEVA
│   │   ├── Pages/
│   │   │   ├── LoginPage       ← Login con DNI
│   │   │   ├── EleccionPage    ← Menú: escanear QR / ver asistencias
│   │   │   └── MisAsistenciasPage ← Lista de asistencias del alumno
│   │   ├── Services/
│   │   │   └── ApiService.cs   ← Llamadas HTTP a la Web API
│   │   ├── Models/
│   │   │   └── AsistenciaItem.cs
│   │   └── QRAsistencia.MAUI.csproj
│   │
│   ├── WebApi/                 ← Backend REST (.NET 8, C#)
│   │   ├── Asistencia.API/     ← Controllers, configuración
│   │   ├── Asistencia.Application/ ← Servicios e interfaces
│   │   ├── Asistencia.Domain/  ← Entidades y DTOs
│   │   ├── Asistencia.Infrastructure/ ← Repositorios, DbContext
│   │   └── Asistencia.sln
│   │
│   ├── Microcontroladores/
│   │   └── AsistenciaQR/       ← Firmware ESP32 (Arduino)
│   │       ├── AsistenciaQR.ino
│   │       ├── GeneratorQR.h
│   │       ├── HacerPOST.h
│   │       ├── NumeroRandom.h
│   │       └── Variables.h
│   │
│   └── SQL/
│       └── Instalacion.sql     ← Script para crear la base de datos
│
└── Documentos/                 ← Diagramas y documentación
```

---

## Componentes

### 1. App Móvil — `Codigos/MAUI`

App Android desarrollada en **.NET MAUI (C#)**. Migrada desde el código original en Android Studio (Java).

**Requisitos:**
- Visual Studio 2022 con el workload **.NET MAUI** instalado
- SDK de Android (API 29+)

**Instalación:**
```bash
# Abrir el proyecto
code Codigos/MAUI/QRAsistencia.MAUI.csproj
# o abrir con Visual Studio 2022
```

**Dependencias NuGet** (se restauran automáticamente):
- `Microsoft.Maui.Controls` 8.0.82
- `ZXing.Net.Maui.Controls` 0.4.0 — escaneo de QR con la cámara

**Configuración:** La URL de la API se define en `MauiProgram.cs`:
```csharp
BaseAddress = new Uri("http://77.81.230.76:5095/")
```

**Pantallas:**

| Pantalla | Descripción |
|---|---|
| LoginPage | Alumno ingresa su DNI. La app registra el ID del dispositivo en la API. |
| EleccionPage | Menú con dos opciones: escanear QR para tomar asistencia, o ver mis asistencias. |
| MisAsistenciasPage | Lista con fechas y horas de asistencias registradas. |

---

### 2. Web API — `Codigos/WebApi`

REST API desarrollada en **.NET 8** con arquitectura en capas (Domain / Application / Infrastructure / API) y Entity Framework Core con PostgreSQL.

**Requisitos:**
- .NET 8 SDK
- PostgreSQL (ver sección Base de Datos)

**Instalación:**
```bash
cd Codigos/WebApi
dotnet restore
dotnet run --project Asistencia.API
```

**Configuración** — editar `Asistencia.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "host=TU_HOST; port=5432; Database=QR_Asistencias; user id=TU_USUARIO; Password=TU_PASSWORD"
  }
}
```

**Endpoints:**

| Método | Ruta | Descripción |
|---|---|---|
| `PUT` | `/api/Alumno/mac` | Registra el ID del dispositivo para un DNI |
| `POST` | `/api/RegistroAsistencia` | Registra una asistencia (MAC + código QR) |
| `GET` | `/api/RegistroAsistencia/{dni}` | Retorna las asistencias de un alumno |

**Publicar para producción:**
```bash
cd Codigos/WebApi
dotnet publish --output serverapi
```

---

### 3. Base de Datos — `Codigos/SQL`

PostgreSQL. El script `Instalacion.sql` crea la base de datos y las tablas.

**Requisitos:**
- PostgreSQL instalado y corriendo

**Instalación:**
```sql
-- Ejecutar en psql o pgAdmin:
\i Codigos/SQL/Instalacion.sql
```

**Tablas:**

```sql
-- Alumnos registrados
alumno (dni PK, nombre, apellido, mac UNIQUE)

-- Registro de asistencias
registro_asistencia (registro_id SERIAL PK, dni FK, fecha BIGINT)
-- fecha almacenada como Unix timestamp (segundos)
```

---

### 4. Microcontrolador — `Codigos/Microcontroladores`

Firmware para **ESP32** desarrollado en Arduino IDE. Genera códigos QR aleatorios, los muestra en una pantalla OLED y los envía a la Web API.

**Requisitos:**
- Arduino IDE con soporte para ESP32
- Librerías: ver `Librerias/README.md`

**Instalación:**
1. Abrir `AsistenciaQR/AsistenciaQR.ino` en Arduino IDE
2. Configurar WiFi y URL de la API en `Variables.h`
3. Compilar y subir al ESP32

---

## Flujo completo del sistema

```
1. El alumno abre la app e ingresa su DNI (solo la primera vez)
   → La app registra el ID del dispositivo en la API (PUT /api/Alumno/mac)

2. El microcontrolador genera un QR aleatorio y lo muestra en la pantalla OLED
   → También lo registra en la API (POST desde el ESP32)

3. El alumno selecciona "Tomar Asistencia" y escanea el QR con la cámara
   → La app envía { mac, codigoQR } a la API (POST /api/RegistroAsistencia)
   → La API valida el QR y registra la asistencia en la BD

4. El alumno puede ver sus asistencias en "Mis Asistencias"
   → La app consulta la API (GET /api/RegistroAsistencia/{dni})
```

---

## Variables de entorno / Configuración rápida

| Variable | Archivo | Descripción |
|---|---|---|
| URL de la API | `MAUI/MauiProgram.cs` | Base URL del servidor |
| Cadena de conexión BD | `Asistencia.API/appsettings.json` | Host, usuario y password de PostgreSQL |
| Credenciales WiFi | `Microcontroladores/.../Variables.h` | SSID y contraseña de la red |

---

## Tecnologías

- **App móvil:** .NET MAUI 8, C#, ZXing.Net.MAUI
- **Backend:** .NET 8, ASP.NET Core, Entity Framework Core, PostgreSQL
- **Microcontrolador:** ESP32, Arduino IDE
- **Base de datos:** PostgreSQL
