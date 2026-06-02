using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asistencia.Application.Interfaces;
using Asistencia.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRController : ControllerBase
    {
        private readonly IControlQRServicio _controlQRServicio;
        public QRController(IControlQRServicio controlQRServicio)
        {
            _controlQRServicio = controlQRServicio;
        }
/*---------------------------------------------------------------------------------------------------------------------------*/        
        [HttpPost]
        public IActionResult AgregarQR([FromBody] MicroDTO microDTO)
        {
            if (_controlQRServicio.GuardarQR(microDTO)){
                return Ok("Codigo guardado");
            }else{
                return NotFound("Codigo no guardado");
            }
            
            
        }
    }
}