using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;
using Asistencia.Infrastructure.Data;

namespace Asistencia.Infrastructure.Repository;

public class ControlQRRepository : IControlQRRepository
{
    private readonly ApplicationDbContext _db;

    public ControlQRRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Guarda o actualiza el QR activo en la base de datos (siempre fila id=1).
    /// Así sobrevive reinicios del servidor.
    /// </summary>
    public bool GuardaQR(MicroDTO microDTO)
    {
        try
        {
            var existing = _db.ControlQR.Find(1);
            if (existing == null)
            {
                _db.ControlQR.Add(new ControlQR
                {
                    Id = 1,
                    Key = microDTO.Key,
                    Valor = microDTO.Valor,
                    GeneradoEn = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                });
            }
            else
            {
                existing.Key = microDTO.Key;
                existing.Valor = microDTO.Valor;
                existing.GeneradoEn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            _db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error guardando QR: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Devuelve el QR activo desde la base de datos.
    /// </summary>
    public MicroDTO pedirQR()
    {
        try
        {
            var qr = _db.ControlQR.Find(1);
            if (qr == null) return new MicroDTO();
            return new MicroDTO { Key = qr.Key, Valor = qr.Valor };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo QR: {ex.Message}");
            return new MicroDTO();
        }
    }
}
