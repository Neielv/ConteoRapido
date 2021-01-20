using CoreCRUDwithORACLE.Comunes;
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

        public async Task<IActionResult> Index(string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ProvSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prov_desc" : "";
            ViewData["CurrentFilter"] = textoBuscar;

            int number;

            if (textoBuscar != null)
            {
                pageNumber = 1;
            }
            else
            {
                textoBuscar = currentFilter;
            }

            ViewData["CurrentFilter"] = textoBuscar;

            IEnumerable<AOperadoresProvincia> operadoresProvincias = null;
            int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            int codigoRol = Convert.ToInt32(HttpContext.Session.GetString("cod_rol"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codigoRol != 5)
            {
                if (codigoProvincia == 0)
                    operadoresProvincias = await servicioReportes.OperadoresProvincia();
                else
                    operadoresProvincias = await servicioReportes.OperadoresProvincia(codigoProvincia);
            }
            else
                operadoresProvincias = await servicioReportes.OperadoresProvincia();

            if ((operadoresProvincias == null) || (operadoresProvincias.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    operadoresProvincias = operadoresProvincias.Where(a => a.COD_PROV == number);
                }
                else
                {
                    operadoresProvincias = operadoresProvincias.Where(s => s.PROVINCIA.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "prov_desc":
                    operadoresProvincias = operadoresProvincias.OrderByDescending(a => a.PROVINCIA);
                    break;
                default:
                    operadoresProvincias = operadoresProvincias.OrderBy(a => a.PROVINCIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<AOperadoresProvincia>.CreateAsync(operadoresProvincias.AsQueryable(), pageNumber ?? 1, pageSize));
            //return View(operadoresProvincias);
        }

        [Route("Reportes/OperadoresCanton/{codProvincia}")]
        public async Task<IActionResult> OperadoresCanton(int codProvincia, string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");
            ViewData["CantonSortParm"] = String.IsNullOrEmpty(sortOrder) ? "canton_desc" : "";
            ViewData["ProvSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prov_desc" : "nom_prov";
            ViewData["CurrentFilter"] = textoBuscar;

            int number;

            if (textoBuscar != null)
            {
                pageNumber = 1;
            }
            else
            {
                textoBuscar = currentFilter;
            }

            ViewData["CurrentFilter"] = textoBuscar;

            IEnumerable<AOperadoresCanton> operadoresCanton = null;
            //int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codProvincia == 0)
                operadoresCanton = await servicioReportes.OperadoresCanton();
            else
                operadoresCanton = await servicioReportes.OperadoresCanton(codProvincia);

            if ((operadoresCanton == null) || (operadoresCanton.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    operadoresCanton = operadoresCanton.Where(a => a.COD_CANTON == number);
                }
                else
                {
                    operadoresCanton = operadoresCanton.Where(s => s.CANTON.Contains(textoBuscar)
                                                                || s.operadoresProvincia.PROVINCIA.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "canton_desc":
                    operadoresCanton = operadoresCanton.OrderByDescending(a => a.CANTON);
                    break;
                case "prov_desc":
                    operadoresCanton = operadoresCanton.OrderByDescending(a => a.operadoresProvincia.PROVINCIA);
                    break;
                case "nom_prov":
                    operadoresCanton = operadoresCanton.OrderBy(a => a.operadoresProvincia.PROVINCIA);
                    break;
                default:
                    operadoresCanton = operadoresCanton.OrderBy(a => a.CANTON);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<AOperadoresCanton>.CreateAsync(operadoresCanton.AsQueryable(), pageNumber ?? 1, pageSize));

            //return View(operadoresCanton);
        }

        [Route("Reportes/OperadoresParroquia/{codCanton}")]
        public async Task<IActionResult> OperadoresParroquia(int codCanton, string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            ViewData["CanSortParm"] = String.IsNullOrEmpty(sortOrder) ? "canton_desc" : "nom_cant";
            ViewData["ProvSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prov_desc" : "nom_prov";
            ViewData["ParrSortParm"] = String.IsNullOrEmpty(sortOrder) ? "parr_desc" : ""; 
            ViewData["CurrentFilter"] = textoBuscar;

            int number;

            if (textoBuscar != null)
            {
                pageNumber = 1;
            }
            else
            {
                textoBuscar = currentFilter;
            }

            ViewData["CurrentFilter"] = textoBuscar;

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

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    operadoresParroquia = operadoresParroquia.Where(a => a.PCODIGO == number);
                }
                else
                {
                    operadoresParroquia = operadoresParroquia.Where(s => s.PARROQUIA.Contains(textoBuscar)
                                                                || s.operadoresCanton.operadoresProvincia.PROVINCIA.Contains(textoBuscar)
                                                                || s.operadoresCanton.CANTON.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "parr_desc":
                    operadoresParroquia = operadoresParroquia.OrderByDescending(a => a.PARROQUIA);
                    break;
                case "canton_desc":
                    operadoresParroquia = operadoresParroquia.OrderByDescending(a => a.operadoresCanton.CANTON);
                    break;
                case "nom_cant":
                    operadoresParroquia = operadoresParroquia.OrderBy(a => a.operadoresCanton.CANTON);
                    break;
                case "prov_desc":
                    operadoresParroquia = operadoresParroquia.OrderByDescending(a => a.operadoresCanton.operadoresProvincia.PROVINCIA);
                    break;
                case "nom_prov":
                    operadoresParroquia = operadoresParroquia.OrderBy(a => a.operadoresCanton.operadoresProvincia.PROVINCIA);
                    break;
                default:
                    operadoresParroquia = operadoresParroquia.OrderBy(a => a.PARROQUIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<AOperadoresParroquia>.CreateAsync(operadoresParroquia.AsQueryable(), pageNumber ?? 1, pageSize));
            //return View(operadoresParroquia);
        }

        [Route("Reportes/DetalleOperadores/{codParroquia}")]
        public async Task<IActionResult> DetalleOperadores(int codParroquia, string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");
            ViewData["CanSortParm"] = String.IsNullOrEmpty(sortOrder) ? "canton_desc" : "nom_cant";
            ViewData["ProvSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prov_desc" : "nom_prov";
            ViewData["ParrSortParm"] = String.IsNullOrEmpty(sortOrder) ? "parr_desc" : "";
            ViewData["CurrentFilter"] = textoBuscar;

            int number;

            if (textoBuscar != null)
            {
                pageNumber = 1;
            }
            else
            {
                textoBuscar = currentFilter;
            }

            ViewData["CurrentFilter"] = textoBuscar;

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

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    operadoresDetalle = operadoresDetalle.Where(a => a.CODIGO == number);
                }
                else
                {
                    operadoresDetalle = operadoresDetalle.Where(s => s.PARROQUIA.Contains(textoBuscar)
                                                                || s.PROVINCIA.Contains(textoBuscar)
                                                                || s.CANTON.Contains(textoBuscar)
                                                                || s.USUARIO.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "parr_desc":
                    operadoresDetalle = operadoresDetalle.OrderByDescending(a => a.PARROQUIA);
                    break;
                case "canton_desc":
                    operadoresDetalle = operadoresDetalle.OrderByDescending(a => a.CANTON);
                    break;
                case "nom_cant":
                    operadoresDetalle = operadoresDetalle.OrderBy(a => a.CANTON);
                    break;
                case "prov_desc":
                    operadoresDetalle = operadoresDetalle.OrderByDescending(a => a.PROVINCIA);
                    break;
                case "nom_prov":
                    operadoresDetalle = operadoresDetalle.OrderBy(a => a.PROVINCIA);
                    break;
                default:
                    operadoresDetalle = operadoresDetalle.OrderBy(a => a.PARROQUIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<DetalleOperadores>.CreateAsync(operadoresDetalle.AsQueryable(), pageNumber ?? 1, pageSize));
            
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
        [Route("Reportes/TransmitidasParroquia/{codCanton}")]
        public async Task<IActionResult> TransmitidasParroquia(int codCanton)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            IEnumerable<ATransmitidasParroquias> transmitidasParroquia = null;
            //int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codCanton == 0)
                transmitidasParroquia = await servicioReportes.TransmitidasParroquia();
            else
                transmitidasParroquia = await servicioReportes.TransmitidasParroquia(codCanton);

            if ((transmitidasParroquia == null) || (transmitidasParroquia.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            return View(transmitidasParroquia);
        }
    }
}
