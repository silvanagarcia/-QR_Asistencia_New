using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;

namespace Asistencia.Application
{
    public class ControlQRServicio : IControlQRServicio
    {
        private readonly IControlQRRepository _controlQRRepository;
        public ControlQRServicio(IControlQRRepository controlQRRepository)
        {
            _controlQRRepository = controlQRRepository;
        }

        public bool GuardarQR(MicroDTO microDTO)
        {
            if (string.IsNullOrEmpty(microDTO.Key) || string.IsNullOrEmpty(microDTO.Valor)){
                throw new InvalidDataException("Key y Valor son obligatorios.");
            }
            if (_controlQRRepository.GuardaQR(microDTO)){
                return true;
            }else{
                return false;
            }
            
        }

        MicroDTO IControlQRServicio.ObtenerQR()
        {
            return _controlQRRepository.pedirQR();
        }
    }
}