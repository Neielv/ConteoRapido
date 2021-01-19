﻿using CoreCRUDwithORACLE.Comunes;
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
        public async Task<IActionResult> TransmitidasProvincia(string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

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

            int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            int codigoRol = Convert.ToInt32(HttpContext.Session.GetString("cod_rol"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            IEnumerable<ATransmitidasProvincia> transmitidasProvincias = null;
            
            if (codigoRol != 5)
            {
                if (codigoProvincia == 0)
                    transmitidasProvincias = await servicioReportes.TransmitidasProvincia();
                else
                    transmitidasProvincias = await servicioReportes.TransmitidasProvincia(codigoProvincia);
            }
            else
                transmitidasProvincias = await servicioReportes.TransmitidasProvincia();
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");


            if ((transmitidasProvincias == null) || (transmitidasProvincias.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    transmitidasProvincias = transmitidasProvincias.Where(a => a.CODIGO == number);
                }
                else
                {
                    transmitidasProvincias = transmitidasProvincias.Where(s => s.PROVINCIA.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "prov_desc":
                    transmitidasProvincias = transmitidasProvincias.OrderByDescending(a => a.PROVINCIA);
                    break;
                default:
                    transmitidasProvincias = transmitidasProvincias.OrderBy(a => a.PROVINCIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<ATransmitidasProvincia>.CreateAsync(transmitidasProvincias.AsQueryable(), pageNumber ?? 1, pageSize));
            //return View(transmitidasProvincias);
        }
        [Route("Reportes/TransmitidasCanton/{codProvincia}")]
        public async Task<IActionResult> TransmitidasCanton(int codProvincia, string sortOrder, string currentFilter,
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

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    transmitidasCanton = transmitidasCanton.Where(a => a.CODIGO == number);
                }
                else
                {
                    transmitidasCanton = transmitidasCanton.Where(s => s.CANTON.Contains(textoBuscar)
                                                                || s.PROVINCIA.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "canton_desc":
                    transmitidasCanton = transmitidasCanton.OrderByDescending(a => a.CANTON);
                    break;
                case "prov_desc":
                    transmitidasCanton = transmitidasCanton.OrderByDescending(a => a.PROVINCIA);
                    break;
                case "nom_prov":
                    transmitidasCanton = transmitidasCanton.OrderBy(a => a.PROVINCIA);
                    break;
                default:
                    transmitidasCanton = transmitidasCanton.OrderBy(a => a.CANTON);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<ATransmitidasCanton>.CreateAsync(transmitidasCanton.AsQueryable(), pageNumber ?? 1, pageSize));

        }
        [Route("Reportes/TransmitidasParroquia/{codCanton}")]
        public async Task<IActionResult> TransmitidasParroquia(int codCanton, string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            ViewData["CantonSortParm"] = String.IsNullOrEmpty(sortOrder) ? "canton_desc" : "nom_cant";
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

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    transmitidasParroquia = transmitidasParroquia.Where(a => a.CODIGO == number);
                }
                else
                {
                    transmitidasParroquia = transmitidasParroquia.Where(s => s.PARROQUIA.Contains(textoBuscar)
                                                                || s.PROVINCIA.Contains(textoBuscar)
                                                                || s.CANTON.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "parr_desc":
                    transmitidasParroquia = transmitidasParroquia.OrderByDescending(a => a.PARROQUIA);
                    break;
                case "canton_desc":
                    transmitidasParroquia = transmitidasParroquia.OrderByDescending(a => a.CANTON);
                    break;
                case "nom_cant":
                    transmitidasParroquia = transmitidasParroquia.OrderBy(a => a.CANTON);
                    break;
                case "prov_desc":
                    transmitidasParroquia = transmitidasParroquia.OrderByDescending(a => a.PROVINCIA);
                    break;
                case "nom_prov":
                    transmitidasParroquia = transmitidasParroquia.OrderBy(a => a.PROVINCIA);
                    break;
                default:
                    transmitidasParroquia = transmitidasParroquia.OrderBy(a => a.PARROQUIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<ATransmitidasParroquias>.CreateAsync(transmitidasParroquia.AsQueryable(), pageNumber ?? 1, pageSize));
            //return View(transmitidasParroquia);
        }
        [Route("Reportes/TransmitidasDetalle/{codParroquia}")]
        public async Task<IActionResult> TransmitidasDetalle(int codParroquia, string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            ViewData["CantonSortParm"] = String.IsNullOrEmpty(sortOrder) ? "canton_desc" : "nom_cant";
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

            IEnumerable<DetallesTransmitidas> transmitidasParroquia = null;
            //int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codParroquia == 0)
                transmitidasParroquia = await servicioReportes.TransmitidasDetalle();
            else
                transmitidasParroquia = await servicioReportes.TransmitidasDetalle(codParroquia);

            if ((transmitidasParroquia == null) || (transmitidasParroquia.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    transmitidasParroquia = transmitidasParroquia.Where(a => a.CODIGO == number);
                }
                else
                {
                    transmitidasParroquia = transmitidasParroquia.Where(s => s.PARROQUIA.Contains(textoBuscar)
                                                                || s.PROVINCIA.Contains(textoBuscar)
                                                                || s.CANTON.Contains(textoBuscar)
                                                                || s.USUARIO.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "parr_desc":
                    transmitidasParroquia = transmitidasParroquia.OrderByDescending(a => a.PARROQUIA);
                    break;
                case "canton_desc":
                    transmitidasParroquia = transmitidasParroquia.OrderByDescending(a => a.CANTON);
                    break;
                case "nom_cant":
                    transmitidasParroquia = transmitidasParroquia.OrderBy(a => a.CANTON);
                    break;
                case "prov_desc":
                    transmitidasParroquia = transmitidasParroquia.OrderByDescending(a => a.PROVINCIA);
                    break;
                case "nom_prov":
                    transmitidasParroquia = transmitidasParroquia.OrderBy(a => a.PROVINCIA);
                    break;
                default:
                    transmitidasParroquia = transmitidasParroquia.OrderBy(a => a.PARROQUIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<DetallesTransmitidas>.CreateAsync(transmitidasParroquia.AsQueryable(), pageNumber ?? 1, pageSize));
            //return View(transmitidasParroquia);
        }
        [Route("Reportes/GeneralProvincia")]
        public async Task<IActionResult> GeneralProvincia(string sortOrder, string currentFilter,
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

            IEnumerable<InformacionGeneral> generalProvincias = null;
            int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            int codigoRol = Convert.ToInt32(HttpContext.Session.GetString("cod_rol"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codigoRol != 5)
            {
                if (codigoProvincia == 0)
                    generalProvincias = await servicioReportes.GeneralProvincia();
                else
                    generalProvincias = await servicioReportes.GeneralProvincia(codigoProvincia);
            }
            else
                generalProvincias = await servicioReportes.GeneralProvincia();

            if ((generalProvincias == null) || (generalProvincias.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    generalProvincias = generalProvincias.Where(a => a.COD_PROVINCIA == number);
                }
                else
                {
                    generalProvincias = generalProvincias.Where(s => s.NOM_PROVINCIA.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "prov_desc":
                    generalProvincias = generalProvincias.OrderByDescending(a => a.NOM_PROVINCIA);
                    break;
                default:
                    generalProvincias = generalProvincias.OrderBy(a => a.NOM_PROVINCIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<InformacionGeneral>.CreateAsync(generalProvincias.AsQueryable(), pageNumber ?? 1, pageSize));
            
        }
        [Route("Reportes/GeneralCanton/{codigoProvincia}")]
        public async Task<IActionResult> GeneralCanton(int codigoProvincia, string sortOrder, string currentFilter,
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

            IEnumerable<InformacionGeneral> generalesCanton = null;

            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codigoProvincia == 0)
                generalesCanton = await servicioReportes.GeneralCanton();
            else
                generalesCanton = await servicioReportes.GeneralCanton(codigoProvincia);

            if ((generalesCanton == null) || (generalesCanton.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    generalesCanton = generalesCanton.Where(a => a.COD_CANTON == number);
                }
                else
                {
                    generalesCanton = generalesCanton.Where(s => s.NOM_CANTON.Contains(textoBuscar)
                                                                || s.NOM_PROVINCIA.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "canton_desc":
                    generalesCanton = generalesCanton.OrderByDescending(a => a.NOM_CANTON);
                    break;
                case "prov_desc":
                    generalesCanton = generalesCanton.OrderByDescending(a => a.NOM_PROVINCIA);
                    break;
                case "nom_prov":
                    generalesCanton = generalesCanton.OrderBy(a => a.NOM_PROVINCIA);
                    break;
                default:
                    generalesCanton = generalesCanton.OrderBy(a => a.NOM_CANTON);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<InformacionGeneral>.CreateAsync(generalesCanton.AsQueryable(), pageNumber ?? 1, pageSize));

        }
        [Route("Reportes/GeneralesParroquia/{codCanton}")]
        public async Task<IActionResult> GeneralesParroquia(int codCanton, string sortOrder, string currentFilter,
                                                string textoBuscar, int? pageNumber)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("Account/LogOut");
            //return RedirectToPage("Logout", "Account");

            ViewData["ZonSortParm"] = String.IsNullOrEmpty(sortOrder) ? "zon_desc" : "nom_zon";
            ViewData["CantonSortParm"] = String.IsNullOrEmpty(sortOrder) ? "canton_desc" : "nom_cant";
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

            IEnumerable<InformacionGeneral> generalesParroquia = null;
            //int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));
            //if (!codigoProvincia.HasValue)
            //    return RedirectToAction("Logout", "Account");
            if (codCanton == 0)
                generalesParroquia = await servicioReportes.GeneralParroquia();
            else
                generalesParroquia = await servicioReportes.GeneralParroquia(codCanton);

            if ((generalesParroquia == null) || (generalesParroquia.Count() == 0))
            {
                ModelState.AddModelError(string.Empty, "No existen Registros.");
                return View();
            }

            if (!String.IsNullOrEmpty(textoBuscar))
            {
                if (Int32.TryParse(textoBuscar, out number))
                {
                    generalesParroquia = generalesParroquia.Where(a => a.COD_PARROQUIA == number);
                }
                else
                {
                    generalesParroquia = generalesParroquia.Where(s => s.NOM_CANTON.Contains(textoBuscar)
                                                                || s.NOM_PARROQUIA.Contains(textoBuscar)
                                                                || s.NOM_PROVINCIA.Contains(textoBuscar)
                                                                || s.NOM_ZONA.Contains(textoBuscar));
                }
            }

            switch (sortOrder)
            {
                case "parr_desc":
                    generalesParroquia = generalesParroquia.OrderByDescending(a => a.NOM_PARROQUIA);
                    break;
                case "canton_desc":
                    generalesParroquia = generalesParroquia.OrderByDescending(a => a.NOM_CANTON);
                    break;
                case "nom_cant":
                    generalesParroquia = generalesParroquia.OrderBy(a => a.NOM_CANTON);
                    break;
                case "prov_desc":
                    generalesParroquia = generalesParroquia.OrderByDescending(a => a.NOM_PROVINCIA);
                    break;
                case "nom_prov":
                    generalesParroquia = generalesParroquia.OrderBy(a => a.NOM_PROVINCIA);
                    break;
                case "zon_desc":
                    generalesParroquia = generalesParroquia.OrderByDescending(a => a.NOM_ZONA);
                    break;
                case "nom_zon":
                    generalesParroquia = generalesParroquia.OrderBy(a => a.NOM_ZONA);
                    break;
                default:
                    generalesParroquia = generalesParroquia.OrderBy(a => a.NOM_PARROQUIA);
                    break;
            }

            int pageSize = 10;

            return View(await PaginatedListAsync<InformacionGeneral>.CreateAsync(generalesParroquia.AsQueryable(), pageNumber ?? 1, pageSize));
            //return View(transmitidasParroquia);
        }

    }
}
