using Asistencia.Domain.Entities;

namespace Asistencia.Application.Interfaces;

public interface IAlumnoServicio
{
    IEnumerable<Alumno> ObtenerAlumnos();
    Alumno ObtenerAlumnoPorDNI(int dni);
    Alumno ObtenerPorMac(string mac);
    bool AgregarAlumno(Alumno alumno);
    bool ActualizarAlumno(Alumno alumno);
    bool ActualizarAlumnoMac(AlumnoDTO alumnoDTO);
    bool EliminarAlumno(int id);
}
