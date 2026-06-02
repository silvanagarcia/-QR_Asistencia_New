using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Application;

public class AlumnoServicio : IAlumnoServicio
{
    private readonly IAlumnoRepository _alumnoRepository;
    public AlumnoServicio(IAlumnoRepository alumnoRepository)
    {
        _alumnoRepository = alumnoRepository;
    }
/////////////////////////////////////////////////////
    public bool ActualizarAlumno(Alumno alumno)
    {
        var alumnoIS = ObtenerAlumnoPorDNI(alumno.DNI);
        if (alumnoIS != null)
        {
            if (_alumnoRepository.UpdateAlumno(alumno)){
                return true;
            }
        }
        return false;
        
    }
/////////////////////////////////////////////////////
    public bool ActualizarAlumnoMac(AlumnoDTO alumnoDTO){

        var alumno =  ObtenerAlumnoPorDNI(alumnoDTO.DNI);
        var alumnaMac = ObtenerPorMac(alumnoDTO.MAC);
        
        if (alumno != null ){

            if (alumnaMac == null){

                alumno.MAC = alumnoDTO.MAC;
                if(ActualizarAlumno(alumno) ){

                    return true;
                }
            }else if(alumnaMac.DNI == alumno.DNI){

                return true;
            }
        }
        return false;
        
    }

    /////////////////////////////////////////////////////
    public bool AgregarAlumno(Alumno alumno)
    {
        var alumnoExiste = ObtenerAlumnoPorDNI(alumno.DNI);
        if (alumnoExiste == null){

            if (!String.IsNullOrEmpty(alumno.Nombre) && !String.IsNullOrEmpty(alumno.Apellido)){

                if (_alumnoRepository.AddAlumno(alumno)){

                    return true;
                }
            }
        }
        return false;
    }
/////////////////////////////////////////////////////
    public bool EliminarAlumno(int id)
    {
        var alumno = ObtenerAlumnoPorDNI(id);
        if (alumno == null){
            throw new InvalidDataException("El alumno no existe");
        }
        _alumnoRepository.DeleteAlumno(id);
        return true;
    }
/////////////////////////////////////////////////////
    public Alumno ObtenerAlumnoPorDNI(int dni)
    {
        return _alumnoRepository.GetAlumnoById(dni);
    }
/////////////////////////////////////////////////////
    public IEnumerable<Alumno> ObtenerAlumnos()
    {
        return _alumnoRepository.GetAlumnos();
    }
/////////////////////////////////////////////////////
    public Alumno ObtenerPorMac(string mac)
    {
        return _alumnoRepository.GetByMac(mac);
    }
}
