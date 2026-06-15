using Asistencia.Domain.Entities;

namespace Asistencia.Application.Interfaces
{
    public interface IControlQRRepository
    {
        bool GuardaQR(MicroDTO microDTO);
        MicroDTO pedirQR();
        bool ValidarQR(string key, string valor);
    }
}
