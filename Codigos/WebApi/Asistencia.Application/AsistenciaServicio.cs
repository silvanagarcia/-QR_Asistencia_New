using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Application;

public class AsistenciaServicio : IAsistenciaServicio
{
    //public TimeSpan hInicio= new TimeSpan(19, 30,0);
    //public TimeSpan hfin= new TimeSpan(22, 0,0);

    private readonly IAsistenciaRepository _registroAsistenciaRepository;
    private readonly IAlumnoServicio _alumnoServicio;
    private readonly IControlQRServicio _controlQRServicio;

    public AsistenciaServicio(IAsistenciaRepository registroAsistenciaRepository, IAlumnoServicio alumnoServicio, IControlQRServicio controlQRServicio)
    {
        _registroAsistenciaRepository = registroAsistenciaRepository;
        _alumnoServicio = alumnoServicio;
        _controlQRServicio = controlQRServicio;
    }
/////////////////////////////////////////////////////////////////////////////////////
    public IEnumerable<AsistenciaAlumno> PedirAsistenciaPorDNI(int dni)
    {
        return _registroAsistenciaRepository.ObtenerAsistenciaPorId(dni);
    }
/////////////////////////////////////////////////////////////////////////////////////
    public bool TomarAsistenciaPorDNI(AsistenciaDTO asistenciaDTO)
    {
        var alumno = _alumnoServicio.ObtenerPorMac(asistenciaDTO.MAC);
        string[] parte = asistenciaDTO.CodigoQR.Split('-');
        if (alumno != null){
            var microDTO  = _controlQRServicio.ObtenerQR();
            if(microDTO.Key == parte[0] && microDTO.Valor == parte[1]){
                
                 AsistenciaAlumno s = new AsistenciaAlumno();
                
                s.Alumno = alumno;
                //s.AlumnoDNI = alumno.DNI;
                s.Fecha = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                var ultima = _registroAsistenciaRepository.ObtenerUltimaAsistencia( alumno.DNI);
                
                if (ultima != null){
                    DateTime dateUltimoUtc = DateTimeOffset.FromUnixTimeSeconds(ultima.Fecha).UtcDateTime;
                    DateTime dateHoyUtc = DateTimeOffset.FromUnixTimeSeconds(s.Fecha).UtcDateTime;

                    // Definir la zona horaria de Argentina
                    TimeZoneInfo argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
                    
                    
                    // Convertir la hora UTC a la hora de Argentina
                    DateTime dateUltimo = TimeZoneInfo.ConvertTimeFromUtc(dateUltimoUtc, argentinaTimeZone);
                    DateTime dateHoy = TimeZoneInfo.ConvertTimeFromUtc(dateHoyUtc, argentinaTimeZone);


                    var diaGuardado = dateUltimo.ToString("yyyy-MM-dd");
                    var diaHoy = dateHoy.ToString("yyyy-MM-dd");

                    //TimeSpan hHoy = dateHoy.TimeOfDay;
                    /*
                    if (!diaHoy.Equals(diaGuardado)){
                        _registroAsistenciaRepository.RegistrarAsistencia(s);
                        return true;
                        //if (hHoy >= hInicio && hHoy  <= hfin){}
                    }
                    */
                    _registroAsistenciaRepository.RegistrarAsistencia(s);
                    return true;
                    
                }else{
                    _registroAsistenciaRepository.RegistrarAsistencia(s);
                    return true;
                }
                
            }
        }
        return false;
    }
}
