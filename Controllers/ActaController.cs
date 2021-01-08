
using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Controllers
{
    public class ActaController : Controller
    {
        IServicioActa servicioActa;
        private readonly IServicioUsuario servicioUsuario;

        public ActaController(IServicioActa _servicioActa, IServicioUsuario _servicioUsuario)
        {
            servicioActa = _servicioActa;
            servicioUsuario = _servicioUsuario;
        }
        // GET: ActaController
        public ActionResult Index()
        {
            IEnumerable<ActaResponse> actas = servicioActa.GetActas(Convert.ToInt32(HttpContext.Session.GetString("cod_provincia")));

            if (actas == null)
            {
                ModelState.AddModelError(string.Empty, "No existen actas");
                return View();
            }

            return View(actas);
        }

        // GET: ActaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ActaController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ActaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ActaController/Edit/5
        //public ActionResult Edit(int id)
        //public ActionResult Edit(string searchString)
        //{
        //    //Acta acta = servicioActa.GetActa(id);
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        Acta acta = servicioActa.GetActa(Convert.ToInt32(searchString));
        //        return View(acta);
        //    }
        //    return View();
        //}
        public ActionResult Edit(string id)
        {
            //Acta acta = servicioActa.GetActa(id);
            if (!String.IsNullOrEmpty(id))
            {
                ActaResponse acta = servicioActa.GetActa(Convert.ToInt32(id));
                return View(acta);
            }
            return View();
        }

        // POST: ActaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ActaResponse collection)
        {
            try
            {
                Usuario usuario = servicioUsuario.GetUsuario(collection.CEDULA);
                if (usuario != null && usuario.COD_PROVINCIA == Convert.ToInt32(HttpContext.Session.GetString("cod_provincia")))
                {
                    collection.COD_USUARIO = usuario.COD_USUARIO;
                    int respuesta = servicioActa.ActualizaActa(collection.COD_USUARIO, collection.COD_JUNTA);
                    if (respuesta > 0)
                        return RedirectToAction(nameof(Index));
                    else
                        return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Operador no pertenece a la provincia");
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: ActaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ActaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
