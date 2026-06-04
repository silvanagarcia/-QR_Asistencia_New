using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Infrastructure.Repository;

public class ControlQRRepository : IControlQRRepository
{
    // Estado en memoria: clave->valor del QR activo
    private static readonly Dictionary<string, string> _codes = new();

    public bool GuardaQR(MicroDTO microDTO)
    {
        try
        {
            if (string.IsNullOrEmpty(microDTO.Key) || string.IsNullOrEmpty(microDTO.Valor))
                return false;

            _codes[microDTO.Key] = microDTO.Valor;
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
        var microDTO = new MicroDTO();
        foreach (var kvp in _codes)
        {
            microDTO.Key = kvp.Key;
            microDTO.Valor = kvp.Value;
        }
        return microDTO;
    }
}
