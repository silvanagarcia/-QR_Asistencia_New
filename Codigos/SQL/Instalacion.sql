--para el server del postgres
--77.81.230.79
--pasword:postgres2024

-- Database: QR_Asistencias
-- DROP DATABASE IF EXISTS "QR_Asistencias";
CREATE DATABASE "QR_Asistencias"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Spanish_Argentina.1252'
    LC_CTYPE = 'Spanish_Argentina.1252'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

-- Crea tabla de Alumno
create table public.alumno(
	dni integer primary key,
	nombre varchar (30) not null,
	apellido varchar (30) not null,
	mac varchar (20) UNIQUE null
);
-- Crea table de Registro
create table public.registro_asistencia(
	registro_id serial primary key,
	dni integer,
	fecha bigint,
	
  	CONSTRAINT fk_alumno_dni
		FOREIGN KEY (dni)
		REFERENCES public.alumno(dni)
);

-- la fecha  esta en bigint y no en timestamp, porque desde el web server le pasamos un long, que el timestamp no acepta
--la siginte linea es para que la mac de cada alumno sea unico y que quede de esta manera
--	mac varchar (20) UNIQUE null

--otorgar permisos de usuario que creamos en la tabla alumno
GRANT ALL ON alumno TO danilo;
--otorgar permisos de usuario que creamos en la tabla registro_asistencia
GRANT ALL ON registro_asistencia TO danilo;



--Cuando defines un campo como SERIAL en PostgreSQL, el motor crea una secuencia que genera los valores incrementales 
--para ese campo. 
--Si un usuario intenta insertar un nuevo registro y no tiene permiso para acceder a esta secuencia,

-- otorgar permisos de uso de la secuencia al usuario que está intentando insertar registros en la tabla
GRANT USAGE, SELECT ON SEQUENCE public.registro_asistencia_registro_id_seq TO danilo;



-------------------------------------------------------------------------------------------------------------------------------------------------------
--inserta datos en la tabla de alumno
insert into public.alumno values (45882898, 'Danilo', 'Guerra');

-- elimina todos lo alumnos
delete from public.alumno;

--elimina todos lo registros de asistencia
delete from public.registro_asistencia;

--esta sentencia genera una carpeta el cuan tendra los archivos nesesario para suvir la api al server
--dotnet publish --output serverapi

--D:\QR_Asistencia\Codigos\WebApi> dotnet publish --output serverapi
--serverapi es la carpeta que se generara y la cual tendra los archivos

23636838
Mauricio
Orellana