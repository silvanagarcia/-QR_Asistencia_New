using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Infrastructure.Repository;

/// <summary>
/// Almacena el QR activo en memoria.
/// El ESP8266 debe hacer POST /api/QR cada vez que el servidor arranca.
/// Para persistencia entre reinicios, ejecutar el SQL de control_qr y
/// habilitar la versión con base de datos.
/// </summary>
public class ControlQRRepository : IControlQRRepository
{
    private static string _key = string.Empty;
    private static string _valor = string.Empty;

    public bool GuardaQR(MicroDTO microDTO)
    {
        try
        {
            if (string.IsNullOrEmpty(microDTO.Key) || string.IsNullOrEmpty(microDTO.Valor))
                return false;

            _key = microDTO.Key;
            _valor = microDTO.Valor;
            Console.WriteLine($"QR activo: {_key}-{_valor}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public MicroDTO pedirQR()
    {
        return new MicroDTO { Key = _key, Valor = _valor };
    }
}
