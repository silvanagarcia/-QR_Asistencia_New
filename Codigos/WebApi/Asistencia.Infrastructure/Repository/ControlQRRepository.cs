using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Infrastructure.Repository;

/// <summary>
/// Almacena los últimos 5 códigos QR generados por el ESP8266.
/// Acepta cualquiera de ellos como válido, evitando el 404
/// cuando el QR rota mientras el alumno está escaneando.
/// </summary>
public class ControlQRRepository : IControlQRRepository
{
    private const int MAX_HISTORIAL = 5;

    // Lista de los últimos N códigos: cada entrada es (Key, Valor)
    private static readonly List<(string Key, string Valor)> _historial = new();
    private static readonly object _lock = new();

    public bool GuardaQR(MicroDTO microDTO)
    {
        try
        {
            if (string.IsNullOrEmpty(microDTO.Key) || string.IsNullOrEmpty(microDTO.Valor))
                return false;

            lock (_lock)
            {
                _historial.Add((microDTO.Key, microDTO.Valor));

                // Mantener solo los últimos N
                if (_historial.Count > MAX_HISTORIAL)
                    _historial.RemoveAt(0);
            }

            Console.WriteLine($"[QR] Nuevo código: {microDTO.Key}-{microDTO.Valor} | Historial: {_historial.Count} códigos");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[QR] Error guardando: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Valida si el QR escaneado coincide con alguno de los últimos códigos.
    /// </summary>
    public bool ValidarQR(string key, string valor)
    {
        lock (_lock)
        {
            return _historial.Any(q =>
                q.Key.Equals(key, StringComparison.OrdinalIgnoreCase) &&
                q.Valor.Equals(valor, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Devuelve el QR más reciente.
    /// </summary>
    public MicroDTO pedirQR()
    {
        lock (_lock)
        {
            if (_historial.Count == 0) return new MicroDTO();
            var ultimo = _historial.Last();
            return new MicroDTO { Key = ultimo.Key, Valor = ultimo.Valor };
        }
    }
}
