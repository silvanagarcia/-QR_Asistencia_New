using Asistencia.Domain.Entities;

namespace Asistencia.Application.Interfaces
{
    public interface IControlQRServicio
    {
        bool GuardarQR(MicroDTO microDTO);
        MicroDTO ObtenerQR();
        bool ValidarQR(string key, string valor);
    }
}
