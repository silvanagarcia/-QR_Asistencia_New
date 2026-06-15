using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Application;

public class AsistenciaServicio : IAsistenciaServicio
{
    private readonly IAsistenciaRepository _registroAsistenciaRepository;
    private readonly IAlumnoServicio _alumnoServicio;
    private readonly IControlQRServicio _controlQRServicio;

    public AsistenciaServicio(
        IAsistenciaRepository registroAsistenciaRepository,
        IAlumnoServicio alumnoServicio,
        IControlQRServicio controlQRServicio)
    {
        _registroAsistenciaRepository = registroAsistenciaRepository;
        _alumnoServicio = alumnoServicio;
        _controlQRServicio = controlQRServicio;
    }

    public IEnumerable<AsistenciaAlumno> PedirAsistenciaPorDNI(int dni)
        => _registroAsistenciaRepository.ObtenerAsistenciaPorId(dni);

    public bool TomarAsistenciaPorDNI(AsistenciaDTO asistenciaDTO)
    {
        // 1. Buscar alumno por MAC del dispositivo
        var alumno = _alumnoServicio.ObtenerPorMac(asistenciaDTO.MAC);
        if (alumno == null) return false;

        // 2. Separar el código QR en KEY y VALOR (formato: "001-123456")
        var partes = asistenciaDTO.CodigoQR.Split('-');
        if (partes.Length < 2) return false;

        string key   = partes[0];
        string valor = partes[1];

        // 3. Validar contra el historial de QR (acepta los últimos 5 generados)
        //    Esto evita el 404 cuando el QR rota mientras el alumno escanea
        if (!_controlQRServicio.ValidarQR(key, valor)) return false;

        // 4. Registrar la asistencia
        var registro = new AsistenciaAlumno
        {
            Alumno = alumno,
            Fecha  = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        _registroAsistenciaRepository.RegistrarAsistencia(registro);
        return true;
    }
}
