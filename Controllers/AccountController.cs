using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using CoreCRUDwithORACLE.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Controllers
{
    public class AccountController : Controller
    {
        private readonly IServicioUsuario usuarioManager;

        //private readonly UserManager<IdentityUser> userManager;
        //private readonly SignInManager<IdentityUser> signInManager;

        //public AccountController(UserManager<IdentityUser> userManager,
        //                         SignInManager<IdentityUser> signInManager,
        //                         IServicioUsuario usuarioManager)
        //{
        //    this.userManager = userManager;
        //    this.signInManager = signInManager;
            
        //}

        public AccountController(IServicioUsuario usuarioManager)
        {
            this.usuarioManager = usuarioManager;
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        //        var result = await userManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            await signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("index", "home");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                Login result = usuarioManager.GetAutenticacionUsuario(model.Email, model.Password);

                if (result != null)
                {
                    //var resultado = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    //ViewBag.Model = model;
                    //if (resultado.Succeeded)
                    HttpContext.Session.SetString("cod_rol", result.COD_ROL.ToString());
                    if (result.EST_CLAVE == 0)
                        return RedirectToAction("ActualizaClave", new RouteValueDictionary(
                                                        new { controller = "Usuario", action = "ActualizaClave", cedula = result.CEDULA }));

                    
                    HttpContext.Session.SetString("cod_provincia", result.COD_PROVINCIA.ToString());
                    ViewBag.CODROL = result.COD_ROL;
                    if (result.COD_ROL == 4)
                    { 
                        ModelState.AddModelError(string.Empty, "Intento de ingreso inválido");
                        return View(model);
                    }
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Intento de ingreso inválido");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            //await signInManager.SignOutAsync();
            HttpContext.Session.SetString("cod_rol", "");
            return RedirectToAction("Login", "Account");
        }

        //[HttpPost]
        //public IActionResult Logout()
        //{
        //    //await signInManager.SignOutAsync();
        //    HttpContext.Session.SetString("cod_rol", null);
        //    return RedirectToAction("Login", "Account");
        //}
    }
}
