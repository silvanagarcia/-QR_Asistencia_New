using Asistencia.Domain.Entities;

namespace Asistencia.Application.Interfaces;

public interface IAlumnoRepository
{
    IEnumerable<Alumno> GetAlumnos();
    Alumno GetAlumnoById(int id);
    Alumno GetByMac(string mac);
    bool AddAlumno(Alumno alumno);
    bool UpdateAlumno(Alumno alumno);
    void DeleteAlumno(int id);
}
