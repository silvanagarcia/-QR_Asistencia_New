using Asistencia.Domain.Entities;

namespace Asistencia.Application.Interfaces;

public interface IAsistenciaServicio
{
    IEnumerable<AsistenciaAlumno> PedirAsistenciaPorDNI(int dni);


    bool TomarAsistenciaPorDNI(AsistenciaDTO asistenciaDTO);
    
}
