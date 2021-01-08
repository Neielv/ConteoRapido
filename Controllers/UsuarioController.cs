using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Controllers
{
    public class UsuarioController : Controller
    {
        //[Produces("application/json")]
        IServicioUsuario servicioUsuario;
        private readonly ApplicationUser auc;

        public UsuarioController(IServicioUsuario _servicioUsuario, ApplicationUser auc)
        {
            servicioUsuario = _servicioUsuario;
            this.auc = auc;
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Index()
        {

            IEnumerable<Usuario> usuarios = servicioUsuario.GetUsuarios(Convert.ToInt32(HttpContext.Session.GetString("cod_rol")),
                                                                        Convert.ToInt32(HttpContext.Session.GetString("cod_provincia")));
            if (usuarios == null)
            {
                ModelState.AddModelError(string.Empty, "No existen usuarios");
                return View();
            }

            return View(usuarios);

        }

        public ActionResult Edit(string id)
        {
            //Usuario usuario = servicioUsuario.GetUsuario(id);
            //if (usuario != null)
            //    return View(usuario);
            //else
            //    return View();
            //Usuario usuarioActualizado = servicioUsuario.ObtieneUsuario(usuario);

            int codigoRol = Convert.ToInt32(HttpContext.Session.GetString("cod_rol"));
            int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));

            Usuario usuario = servicioUsuario.GetUsuario(id);

            if (usuario == null)
                return View();

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
            usuarioViewModel.CEDULA = usuario.CEDULA.Substring(0, 9);
            usuarioViewModel.COD_USUARIO = usuario.COD_USUARIO;
            usuarioViewModel.ESTADO = usuario.ESTADO;
            usuarioViewModel.DIGITO = usuario.CEDULA.Substring(9, 1);
            usuarioViewModel.LOGEO = usuario.LOGEO;
            usuarioViewModel.MAIL = usuario.MAIL;
            usuarioViewModel.NOMBRE = usuario.NOMBRE;
            usuarioViewModel.PROVINCIA = usuario.PROVINCIA;
            usuarioViewModel.ROL = usuario.ROL;

            var provincias = (from Provincia in auc.PROVINCIA
                          where Provincia.COD_PROVINCIA == usuario.COD_PROVINCIA
                          orderby Provincia.NOM_PROVINCIA
                          select new SelectListItem()
                          {
                              Text = Provincia.NOM_PROVINCIA,
                              Value = Provincia.COD_PROVINCIA.ToString()
                          }).ToList();

            usuarioViewModel.provincias = provincias;

            var roles = (from Rol in auc.ROL
                     where Rol.COD_ROL == usuario.COD_ROL
                     select new SelectListItem()
                     {
                         Text = Rol.DES_ROL,
                         Value = Rol.COD_ROL.ToString()
                     }).ToList();

            usuarioViewModel.roles = roles;

            //switch (codigoRol)
            //{
            //    case 1:

            //        //var provincias = (from Provincia in auc.PROVINCIA
            //        //                  where Provincia.COD_PROVINCIA > 0 && Provincia.COD_PROVINCIA < 26
            //        //                  orderby Provincia.NOM_PROVINCIA
            //        //                  select new SelectListItem()
            //        //                  {
            //        //                      Text = Provincia.NOM_PROVINCIA,
            //        //                      Value = Provincia.COD_PROVINCIA.ToString()
            //        //                  }).ToList();

            //        //provincias.Find(c => c.Value == usuario.COD_PROVINCIA.ToString()).Selected = true;


            //        var roles = (from Rol in auc.ROL
            //                     where Rol.COD_ROL > 1
            //                     select new SelectListItem()
            //                     {
            //                         Text = Rol.DES_ROL,
            //                         Value = Rol.COD_ROL.ToString()
            //                     }).ToList();

            //        //roles.Find(r => r.Value == usuario.COD_ROL.ToString()).Selected = true;

            //        //usuarioViewModel.provincias = provincias;
            //        usuarioViewModel.roles = roles;
            //        break;
            //    case 2:
            //        provincias = (from Provincia in auc.PROVINCIA
            //                      where Provincia.COD_PROVINCIA > 0 && Provincia.COD_PROVINCIA < 26
            //                      orderby Provincia.NOM_PROVINCIA
            //                      select new SelectListItem()
            //                      {
            //                          Text = Provincia.NOM_PROVINCIA,
            //                          Value = Provincia.COD_PROVINCIA.ToString()
            //                      }).ToList();

            //        provincias.Find(c => c.Value == usuario.COD_PROVINCIA.ToString()).Selected = true;

            //        roles = (from Rol in auc.ROL
            //                 where Rol.COD_ROL == 3 || Rol.COD_ROL == 5 || Rol.COD_ROL == 6
            //                 select new SelectListItem()
            //                 {
            //                     Text = Rol.DES_ROL,
            //                     Value = Rol.COD_ROL.ToString()
            //                 }).ToList();

            //        roles.Find(r => r.Value == usuario.COD_ROL.ToString()).Selected = true;

            //        usuarioViewModel.provincias = provincias;
            //        usuarioViewModel.roles = roles;
            //        break;
            //    case 3:


            //        roles = (from Rol in auc.ROL
            //                 where Rol.COD_ROL == usuario.COD_ROL
            //                 select new SelectListItem()
            //                 {
            //                     Text = Rol.DES_ROL,
            //                     Value = Rol.COD_ROL.ToString()
            //                 }).ToList();
            //        usuarioViewModel.provincias = provincias;
            //        usuarioViewModel.roles = roles;
            //        break;
            //}

            return View(usuarioViewModel);
        }

        [HttpPost]
        public ActionResult Edit(UsuarioViewModel usuarioMod)
        {
            UsuarioResponse usuario = new UsuarioResponse()
            {
                CEDULA = usuarioMod.CEDULA,
                CODIGO_PROVINCIA = usuarioMod.codProvincia,
                CODIGO_ROL = usuarioMod.codRol,
                COD_USUARIO = usuarioMod.COD_USUARIO,
                DIGITO = usuarioMod.DIGITO,
                ESTADO = usuarioMod.ESTADO,
                LOGEO = usuarioMod.LOGEO,
                MAIL = usuarioMod.MAIL,
                NOMBRE = usuarioMod.NOMBRE,
                PROVINCIA = usuarioMod.PROVINCIA,
                ROL = usuarioMod.ROL
            };
            try
            {
                Usuario respuesta = servicioUsuario.ActualizaUsuario(usuario);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar");
                return View();
            }
            //return View(respuesta);
            //if (respuesta != null)
                //return RedirectToPage("/Index");
                
            //else
            //return View();
        }
        public ActionResult IngresaUsuario()
        {
            int codigoRol = Convert.ToInt32(HttpContext.Session.GetString("cod_rol"));
            int codigoProvincia = Convert.ToInt32(HttpContext.Session.GetString("cod_provincia"));

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
            //if (codigoRol == 3)
            //{
            switch (codigoRol)
            {
                case 1:
                    var provincias = (from Provincia in auc.PROVINCIA
                                      where Provincia.COD_PROVINCIA > 0 && Provincia.COD_PROVINCIA < 26
                                      orderby Provincia.NOM_PROVINCIA
                                      select new SelectListItem()
                                      {
                                          Text = Provincia.NOM_PROVINCIA,
                                          Value = Provincia.COD_PROVINCIA.ToString()
                                      }).ToList();

                    provincias.Insert(0, new SelectListItem()
                    {
                        Text = "----Elija Provincia----",
                        Value = string.Empty
                    });

                    var roles = (from Rol in auc.ROL
                                 where Rol.COD_ROL > 1
                                 select new SelectListItem()
                                 {
                                     Text = Rol.DES_ROL,
                                     Value = Rol.COD_ROL.ToString(),
                                     Selected = false
                                 }).ToList();

                    roles.Insert(0, new SelectListItem()
                    {
                        Text = "----Elija Rol----",
                        Value = string.Empty
                    });
                    usuarioViewModel.provincias = provincias;
                    usuarioViewModel.roles = roles;
                    break;
                case 2:
                    provincias = (from Provincia in auc.PROVINCIA
                                      where Provincia.COD_PROVINCIA > 0 && Provincia.COD_PROVINCIA < 26
                                      orderby Provincia.NOM_PROVINCIA
                                      select new SelectListItem()
                                      {
                                          Text = Provincia.NOM_PROVINCIA,
                                          Value = Provincia.COD_PROVINCIA.ToString()
                                      }).ToList();

                    provincias.Insert(0, new SelectListItem()
                    {
                        Text = "----Elija Provincia----",
                        Value = string.Empty
                    });

                    roles = (from Rol in auc.ROL
                                 where Rol.COD_ROL == 3 || Rol.COD_ROL == 5 || Rol.COD_ROL == 6
                             select new SelectListItem()
                                 {
                                     Text = Rol.DES_ROL,
                                     Value = Rol.COD_ROL.ToString(),
                                     Selected = false
                                 }).ToList();

                    roles.Insert(0, new SelectListItem()
                    {
                        Text = "----Elija Rol----",
                        Value = string.Empty
                    });
                    usuarioViewModel.provincias = provincias;
                    usuarioViewModel.roles = roles;

                    break;
                case 3:
                    provincias = (from Provincia in auc.PROVINCIA
                                  where Provincia.COD_PROVINCIA == codigoProvincia
                                  orderby Provincia.NOM_PROVINCIA
                                  select new SelectListItem()
                                  {
                                      Text = Provincia.NOM_PROVINCIA,
                                      Value = Provincia.COD_PROVINCIA.ToString()
                                  }).ToList();

                    roles = (from Rol in auc.ROL
                             where Rol.COD_ROL == 4
                             select new SelectListItem()
                             {
                                 Text = Rol.DES_ROL,
                                 Value = Rol.COD_ROL.ToString(),
                                 Selected = false
                             }).ToList();

                    usuarioViewModel.provincias = provincias;
                    usuarioViewModel.roles = roles;
                    break;
            }
            return View(usuarioViewModel);
        }
        [HttpPost]
        public ActionResult IngresaUsuario(UsuarioViewModel usuarionNew)
        {

            Usuario validacionUsuario = servicioUsuario.GetUsuario(usuarionNew.CEDULA);

            if (validacionUsuario != null)
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario con la cédula ingresada.");
                return View();
            }

            UsuarioResponse usuario = new UsuarioResponse()
            {
                CEDULA = usuarionNew.CEDULAC,
                CODIGO_PROVINCIA = usuarionNew.codProvincia,
                CLAVE = usuarionNew.CLAVE,
                CODIGO_ROL = usuarionNew.codRol,
                COD_USUARIO = usuarionNew.COD_USUARIO,
                ESTADO = true,
                LOGEO = usuarionNew.LOGEO,
                MAIL = usuarionNew.MAIL.ToLower(),
                NOMBRE = usuarionNew.NOMBRE,
                PROVINCIA = usuarionNew.PROVINCIA,
                ROL = usuarionNew.ROL
            };

            int respuesta = servicioUsuario.IngresaUsuario(usuario);
            if (respuesta > 0)
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction("Error", "Shared");
        }
    }
}
