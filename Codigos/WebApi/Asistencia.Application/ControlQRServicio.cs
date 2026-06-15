using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Application;

public class ControlQRServicio : IControlQRServicio
{
    private readonly IControlQRRepository _controlQRRepository;

    public ControlQRServicio(IControlQRRepository controlQRRepository)
    {
        _controlQRRepository = controlQRRepository;
    }

    public bool GuardarQR(MicroDTO microDTO)
    {
        if (string.IsNullOrEmpty(microDTO.Key) || string.IsNullOrEmpty(microDTO.Valor))
            throw new InvalidDataException("Key y Valor son obligatorios.");

        return _controlQRRepository.GuardaQR(microDTO);
    }

    public MicroDTO ObtenerQR() => _controlQRRepository.pedirQR();

    public bool ValidarQR(string key, string valor) =>
        _controlQRRepository.ValidarQR(key, valor);
}
