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
    public class RegistroAsistenciaController : ControllerBase
    {
        //private readonly IAsistenciaRepository _asistenciaRepository;

        private readonly IAsistenciaServicio _registroAsistenciaServicio;
        public RegistroAsistenciaController(IAsistenciaServicio registroAsistenciaServicio /*IAsistenciaRepository asistenciaRepository*/)
        {
            _registroAsistenciaServicio = registroAsistenciaServicio;
            //_asistenciaRepository = asistenciaRepository;
        }
/*---------------------------------------------------------------------------------------------------------------------------*/
        [HttpGet("{dni}")]
        public ActionResult<IEnumerable<AsistenciaAlumno>> GetRegistro(int dni)
        {
            var asistenciat = _registroAsistenciaServicio.PedirAsistenciaPorDNI(dni);///_asistenciaRepository.ObtenerUltimaAsistencia(dni);
            return Ok(asistenciat);
        }
/*---------------------------------------------------------------------------------------------------------------------------*/
        [HttpPost]
        public IActionResult TomarAsistencia([FromBody] AsistenciaDTO asistenciaDTO)
        {
            if (_registroAsistenciaServicio.TomarAsistenciaPorDNI(asistenciaDTO))
            {
                return Ok("Asistencia tomada");
            }else{
                return NotFound();
            }
        }
    }
}