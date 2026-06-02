using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asistencia.Domain.Entities;

namespace Asistencia.Application.Interfaces
{
    public interface IControlQRRepository
    {
        public bool GuardaQR(MicroDTO microDTO);
        public MicroDTO pedirQR();
    }
}