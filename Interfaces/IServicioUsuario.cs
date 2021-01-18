﻿using CoreCRUDwithORACLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Interfaces
{
    public interface IServicioUsuario
    {
        IEnumerable<Usuario> GetUsuarios(int codigoRol, int codigoProvincia);
        Usuario GetUsuario(string iCedula);
        Usuario ActualizaUsuario(UsuarioResponse usuarioActualizado);
        Login GetAutenticacionUsuario(string iMail, string iPass);
        int IngresaUsuario(UsuarioResponse usuario);
        Usuario ActualizaClave(Usuario usuarioNew, int estado);
        int EncerarBase();
    }
}
