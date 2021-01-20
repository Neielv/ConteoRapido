using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using CoreCRUDwithORACLE.ViewModels.Reportes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IServicioReportes servicioReportes;
        private readonly ApplicationUser applicationUser;

        //// GET: ReportesController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: ReportesController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ReportesController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ReportesController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ReportesController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ReportesController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ReportesController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ReportesController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //[ValidateAntiForgeryToken]
        //public async Task
        public ReportesController(IServicioReportes _servicioReportes, ApplicationUser _applicationUser)
        {
            servicioReportes = _servicioReportes;
            applicationUser = _applicationUser;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            IEnumerable<AOperadoresProvincia> operadoresProvincias = null;
            int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codigoProvincia == 0)
                operadoresProvincias = await servicioReportes.OperadoresProvincia();
            else
                operadoresProvincias = await servicioReportes.OperadoresProvincia(codigoProvincia);

            if ((operadoresProvincias == null) || (operadoresProvincias.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            return View(operadoresProvincias);
        }

        [Route("Reportes/OperadoresCanton/{codProvincia}")]
        public async Task<IActionResult> OperadoresCanton (int codProvincia)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            IEnumerable<AOperadoresCanton> operadoresCanton = null;
            //int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codProvincia == 0)
                operadoresCanton = await servicioReportes.OperadoresCanton();
            else
                operadoresCanton = await servicioReportes.OperadoresCanton(codProvincia);

            if ((operadoresCanton == null)||(operadoresCanton.Count()==0) )
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            return View(operadoresCanton);
        }

        [Route("Reportes/OperadoresParroquia/{codCanton}")]
        public async Task<IActionResult> OperadoresParroquia(int codCanton)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            IEnumerable<AOperadoresParroquia> operadoresParroquia = null;
            //int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codCanton == 0)
                operadoresParroquia = await servicioReportes.OperadoresParroquia();
            else
                operadoresParroquia = await servicioReportes.OperadoresParroquia(codCanton);

            if ((operadoresParroquia == null) || (operadoresParroquia.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            return View(operadoresParroquia);
        }

        [Route("Reportes/DetalleOperadores/{codParroquia}")]
        public async Task<IActionResult> DetalleOperadores(int codParroquia)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            IEnumerable<DetalleOperadores> operadoresDetalle = null;
            //int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codParroquia == 0)
                operadoresDetalle = await servicioReportes.OperadoresDetalle();
            else
                operadoresDetalle = await servicioReportes.OperadoresDetalle(codParroquia);

            if ((operadoresDetalle == null) || (operadoresDetalle.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            return View(operadoresDetalle);
        }
        [Route("Reportes/TransmitidasProvincia")]
        public async Task<IActionResult> TransmitidasProvincia()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            IEnumerable<ATransmitidasProvincia> transmitidasProvincias = null;
            int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codigoProvincia == 0)
                transmitidasProvincias = await servicioReportes.TransmitidasProvincia();
            else
                transmitidasProvincias = await servicioReportes.TransmitidasProvincia(codigoProvincia);

            if ((transmitidasProvincias == null) || (transmitidasProvincias.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            return View(transmitidasProvincias);
        }
        [Route("Reportes/TransmitidasCanton/{codProvincia}")]
        public async Task<IActionResult> TransmitidasCanton(int codProvincia)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            IEnumerable<ATransmitidasCanton> transmitidasCanton = null;
            
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codProvincia == 0)
                transmitidasCanton = await servicioReportes.TransmitidasCanton();
            else
                transmitidasCanton = await servicioReportes.TransmitidasCanton(codProvincia);

            if ((transmitidasCanton == null) || (transmitidasCanton.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            return View(transmitidasCanton);
        }
    }
}
