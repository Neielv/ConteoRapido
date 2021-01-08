using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Controllers
{

    [Produces("application/json")]
    public class JuntaController : Controller
    {
        IServicioJunta servicioJunta;
        public  JuntaController(IServicioJunta _servicioJunta)
        {
            servicioJunta = _servicioJunta;
        }
        public ActionResult Index()
        {
            IEnumerable<Junta> junta = servicioJunta.ObtenerJuntas();
            return View(junta);
        }

        [HttpGet]
        [Route("api/GetListadoJuntas")]
        public ActionResult GetListadoJuntas()
        {
            var juntas = servicioJunta.ObtenerJuntas();
            if (juntas == null)
                return NotFound();
            return Ok(juntas);
        }

        //[HttpGet("{iUsuario}/{iEstado}")]
        //[Route("api/GetActa/iUsuario/iEstado")]
        [HttpGet]
        [Route("api/GetActa/{iUsuario}/{iEstado}")]
        public ActionResult GetActa(int iUsuario, int iEstado)
        {
            var acta = servicioJunta.GetActa(iUsuario,iEstado);
            if (acta == null)
                return NotFound();
            return Ok(acta);
        }
    }
}
