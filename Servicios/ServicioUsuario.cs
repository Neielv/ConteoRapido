using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Servicios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly string _conn;
        private Comunes.Auxiliar _helper = new Comunes.Auxiliar();

        private string consultaUsuarios = @"SELECT US.COD_USUARIO, PR.COD_PROVINCIA, PR.NOM_PROVINCIA, US.CED_USUARIO || US.DIG_USUARIO CEDULA, US.LOG_USUARIO,
                                                US.CLA_USUARIO, US.NOM_USUARIO, RO.COD_ROL, RO.DES_ROL, US.EST_USUARIO, 
                                            US.MAI_USUARIO, US.TEL_USUARIO
                                           FROM USUARIO US, PROVINCIA PR, ROL RO
                                           WHERE US.COD_PROVINCIA = PR.COD_PROVINCIA
                                           AND US.COD_ROL = RO.COD_ROL";

        private string consultaUser = @"SELECT US.COD_USUARIO, PR.COD_PROVINCIA, PR.NOM_PROVINCIA, US.CED_USUARIO || US.DIG_USUARIO CEDULA, US.LOG_USUARIO,
                                               US.NOM_USUARIO, RO.COD_ROL, RO.DES_ROL, US.EST_USUARIO, US.MAI_USUARIO
                                           FROM USUARIO US, PROVINCIA PR, ROL RO
                                           WHERE US.COD_PROVINCIA = PR.COD_PROVINCIA
                                           AND US.COD_ROL = RO.COD_ROL
                                           AND CED_USUARIO = {0}";
                                           //AND EST_USUARIO = 1";

        private string consultaLogin = @"select count(*)
                                        from USUARIO
                                        where MAI_USUARIO = '{0}'
                                        and CLA_USUARIO = '{1}'";

        private string consultaUsuario = @"select COD_ROL, NOM_USUARIO, COD_PROVINCIA, EST_CLA_USUARIO, CED_USUARIO || DIG_USUARIO CEDULA
                                        from USUARIO
                                        where MAI_USUARIO = '{0}'
                                        and CLA_USUARIO = '{1}'";

        private string consultaCodUsuario = @"SELECT MAX(COD_USUARIO) Codigo FROM USUARIO";

        private string ingresaUsuario = @"INSERT INTO CONTEORAPIDO2021.USUARIO (COD_USUARIO,COD_ROL, LOG_USUARIO, 
                                           CLA_USUARIO, CED_USUARIO, DIG_USUARIO, NOM_USUARIO, MAI_USUARIO, EST_USUARIO, 
                                           EST_CLA_USUARIO, NIV_USUARIO, COD_PROVINCIA, PRI_LOG_USUARIO, 
                                           INT_LOGIN, TEL_USUARIO, ULT_USUARIO) 
                                           VALUES ({0}, {1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8}, 0, 0,
                                             {9}, NULL, NULL, NULL, NULL)";

        private string actualizaUsuario = @"UPDATE CONTEORAPIDO2021.USUARIO
                                            SET    COD_ROL         = {0},
                                                   LOG_USUARIO     = '{1}',
                                                   CED_USUARIO     = '{2}',
                                                   DIG_USUARIO     = '{3}',
                                                   NOM_USUARIO     = '{4}',
                                                   MAI_USUARIO     = '{5}',
                                                   EST_USUARIO     = '{6}',
                                                   COD_PROVINCIA   =  {7},
                                                   TEL_USUARIO     = '{8)'
                                            WHERE  COD_USUARIO     = {9}";

        private string actualizaUsuarioClave = @"UPDATE CONTEORAPIDO2021.USUARIO
                                                    SET    CLA_USUARIO     = '{0}',
                                                           EST_CLA_USUARIO = 1
                                                    WHERE  CED_USUARIO     = '{1}'";
        public ServicioUsuario(IConfiguration _configuration)
        {
            _conn = _configuration.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<Usuario> GetUsuarios(int codigoRol, int codigoProvincia)
        {
            //List<Usuario> usuariosPrevios = null;
            List<Usuario> usuarios = null;

            using (OracleConnection con = new OracleConnection(_conn))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "PKG_CONTEO_RAPIDO.CONSULTA_USUARIO";
                        switch (codigoRol)
                        {
                            case 1:
                                consultaUsuarios += " AND US.COD_ROL = 2 ";
                                break;
                            case 2:
                                consultaUsuarios += " AND US.COD_ROL in (3,5) ";
                                break;
                            case 3:
                                consultaUsuarios += " AND US.COD_ROL = 4 AND US.COD_PROVINCIA = " + codigoProvincia;
                                break;
                            //case 4:
                            //    consultaUsuarios = consultaUsuarios + " AND US.COD_ROL = 4 ";
                            //    break;
                        }

                        consultaUsuarios += " ORDER BY 1";
                        
                        cmd.CommandText = string.Format(consultaUsuarios);

                        OracleDataReader odr = cmd.ExecuteReader();

                        if (odr.HasRows)
                        {
                            usuarios = new List<Usuario>();
                            while (odr.Read())
                            {
                                Usuario usuario = new Usuario
                                {
                                    CEDULA = Convert.ToString(odr["CEDULA"]),
                                    COD_PROVINCIA = Convert.ToInt32(odr["COD_PROVINCIA"]),
                                    PROVINCIA = Convert.ToString(odr["NOM_PROVINCIA"]),
                                    COD_ROL = Convert.ToInt32(odr["COD_ROL"]),
                                    ROL = Convert.ToString(odr["DES_ROL"]),
                                    COD_USUARIO = Convert.ToInt32(odr["COD_USUARIO"]),
                                    NOMBRE = Convert.ToString(odr["nom_usuario"]),
                                    //CLAVE = Convert.ToString(odr["CLA_USUARIO"]),
                                    TELEFONO = Convert.ToString(odr["TEL_USUARIO"]),
                                    MAIL = Convert.ToString(odr["MAI_USUARIO"]),
                                    LOGEO = Convert.ToString(odr["LOG_USUARIO"]),
                                    ESTADO = (Convert.ToString(odr["EST_USUARIO"])=="1"?true:false)
                                };
                                usuarios.Add(usuario);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return usuarios;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return usuarios;
        }

        public Usuario GetUsuario(string iCedula)
        {
            Usuario usuario = null;

            using (OracleConnection con = new OracleConnection(_conn))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "PKG_CONTEO_RAPIDO.CONSULTA_USUARIO";
                        cmd.CommandText = string.Format(consultaUser, iCedula.Substring(0,9));
                        cmd.BindByName = true;

                        OracleDataReader odr = cmd.ExecuteReader();
                        if (odr.HasRows)
                        {
                            usuario = new Usuario();
                            while (odr.Read())
                            {
                                usuario.COD_USUARIO = Convert.ToInt32(odr["COD_USUARIO"]);
                                usuario.CEDULA = Convert.ToString(odr["CEDULA"]);
                                usuario.COD_PROVINCIA = Convert.ToInt32(odr["COD_PROVINCIA"]);
                                //usuario.CLAVE = Convert.ToString(odr["CLA_USUARIO"]);
                                usuario.ESTADO = (Convert.ToString(odr["EST_USUARIO"]) == "1" ? true : false);
                                usuario.NOMBRE = Convert.ToString(odr["NOM_USUARIO"]);
                                usuario.PROVINCIA = Convert.ToString(odr["NOM_PROVINCIA"]);
                                usuario.LOGEO = Convert.ToString(odr["LOG_USUARIO"]);
                                usuario.COD_ROL = Convert.ToInt32(odr["COD_ROL"]);
                                usuario.ROL = Convert.ToString(odr["DES_ROL"]);
                                usuario.MAIL = Convert.ToString(odr["MAI_USUARIO"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //return usuario;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return usuario;
        }

        public Usuario ActualizaUsuario(UsuarioResponse usuarioActualizado)
        {
            //string clave = string.Empty;
            Usuario usuario = null;

            usuario = GetUsuario(usuarioActualizado.CEDULA);

            if (usuario != null)
            {
                using (OracleConnection con = new OracleConnection(_conn))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.Connection = con;
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandType = CommandType.Text;
                            //cmd.CommandText = "CONSULTA_AUTENTICACION_USUARIO";

                            //clave = _helper.EncodePassword(usuarioActualizado.CLAVE); 
                            cmd.CommandText = string.Format(actualizaUsuario, usuarioActualizado.CODIGO_ROL, usuarioActualizado.LOGEO,
                                                            usuarioActualizado.CEDULA, usuarioActualizado.DIGITO, 
                                                            usuarioActualizado.NOMBRE, usuarioActualizado.MAIL, usuarioActualizado.ESTADO?"1":"0", 
                                                            usuarioActualizado.CODIGO_PROVINCIA, usuarioActualizado.TELEFONO.ToString(), usuarioActualizado.COD_USUARIO);

                            int odr = cmd.ExecuteNonQuery();

                            if (odr > 0)
                                usuario = GetUsuario(usuarioActualizado.CEDULA);
                            else
                                usuario = null;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                            //return usuario = null;
                        }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                            cmd.Dispose();
                        }

                    }
                }
            }

            return usuario;
        }

        public Login GetAutenticacionUsuario(string iMail, string iPass)
        {
            Login logeo = null;
            string clave = string.Empty;

            using (OracleConnection con = new OracleConnection(_conn))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "CONSULTA_AUTENTICACION_USUARIO";
                        //cmd.BindByName = true;
                        clave = _helper.EncodePassword(iPass);
                        //cmd.CommandText = string.Format(consultaLogin, iMail, clave);
                        cmd.CommandText = string.Format(consultaUsuario, iMail, clave);
                        
                        OracleDataReader odr = cmd.ExecuteReader();

                        if (odr.HasRows)
                        {
                            while (odr.Read())
                            {
                                logeo = new Login()
                                {
                                    COD_PROVINCIA = Convert.ToInt32(odr["COD_PROVINCIA"]),
                                    COD_ROL = Convert.ToInt32(odr["COD_ROL"]),
                                    EST_CLAVE = Convert.ToInt32(odr["EST_CLA_USUARIO"]),
                                    CEDULA = Convert.ToString(odr["CEDULA"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return logeo;
                        //throw ex;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return logeo;
        }

        public Usuario PutAutenticacionUsuario(string iMail, string iPass)
        {
            Usuario usuario = new Usuario();


            string clave = string.Empty;

            using (OracleConnection con = new OracleConnection(_conn))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "CONSULTA_AUTENTICACION_USUARIO";
                        //cmd.CommandText = string.Format(consultaUser, iCedula);
                        cmd.BindByName = true;

                        clave = _helper.EncodePassword(iPass);
                        cmd.Parameters.Add("Return_Value", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
                        cmd.Parameters.Add("I_MAIL_USUARIO", OracleDbType.Varchar2, iMail, ParameterDirection.Input);
                        cmd.Parameters.Add("I_CLAVE_USUARIO", OracleDbType.Varchar2, clave, ParameterDirection.Input);
                        cmd.Parameters.Add("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                        //var result = cmd.ExecuteScalar();
                        OracleDataReader odr = cmd.ExecuteReader();
                        if (odr.HasRows)
                        {
                            //while (odr.Read())
                            //{
                            //    //COD_USUARIO, EST_USUARIO, EST_CLA_USUARIO, NIV_USUARIO
                            //    usuario.COD_USUARIO = Convert.ToInt32(odr["cod_usuario"]);
                            //    usuario.EST_USUARIO = Convert.ToString(odr["EST_USUARIO"]);
                            //    usuario.EST_CLA_USUARIO = Convert.ToString(odr["EST_CLA_USUARIO"]);
                            //    usuario.NIV_USUARIO = Convert.ToString(odr["NIV_USUARIO"]);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return usuario;
        }

        public int IngresaUsuario(UsuarioResponse usuario)
        {
            int respuesta = 0;
            using (OracleConnection con = new OracleConnection(_conn))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "PKG_CONTEO_RAPIDO.CONSULTA_USUARIO";
                        cmd.CommandText = string.Format(consultaCodUsuario);
                        OracleDataReader odr = cmd.ExecuteReader();
                        while (odr.Read())
                        {
                            usuario.COD_USUARIO = Convert.ToInt32(odr["Codigo"]) + 1;
                        }

                        cmd.CommandText = string.Format(ingresaUsuario, usuario.COD_USUARIO, usuario.CODIGO_ROL, usuario.LOGEO,
                                                _helper.EncodePassword(usuario.CLAVE), usuario.CEDULA.Substring(0,9), usuario.CEDULA.Substring(9, 1),
                                                usuario.NOMBRE, usuario.MAIL, usuario.ESTADO?"1":"0", usuario.CODIGO_PROVINCIA);

                        respuesta = cmd.ExecuteNonQuery();
                        return respuesta;

                    }
                    catch (Exception ex)
                    {
                        return respuesta;
                        //throw ex;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }
        }

        public Usuario ActualizaClave(Usuario usuarioNew)
        {
            Usuario usuario = null;
            string clave = string.Empty;

            usuario = GetUsuario(usuarioNew.CEDULA);

            if (usuario != null)
            {
                using (OracleConnection con = new OracleConnection(_conn))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.Connection = con;
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandType = CommandType.Text;
                            //cmd.CommandText = "CONSULTA_AUTENTICACION_USUARIO";

                            clave = _helper.EncodePassword(usuarioNew.CLAVE);
                            cmd.CommandText = string.Format(actualizaUsuarioClave, clave,
                                                            usuarioNew.CEDULA.Substring(0,9));

                            int odr = cmd.ExecuteNonQuery();

                            if (odr == 0)
                                usuario = null;
                        }
                        catch (Exception ex)
                        {
                            return usuario = null;
                        }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                            cmd.Dispose();
                        }

                    }
                }
            }

            return usuario;
        }
    }
}
