using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Servicios
{
    public class ServicioActa : IServicioActa
    {
        private readonly string _conn;

        public ServicioActa(IConfiguration _configuration)
        {
            _conn = _configuration.GetConnectionString("OracleDBConnection");
        }

        private string consultaActas = @" SELECT P.COD_PROVINCIA, P.NOM_PROVINCIA, C.COD_CANTON,C.NOM_CANTON,Q.COD_PARROQUIA, 
                                                 Q.NOM_PARROQUIA, Z.COD_ZONA,Z.NOM_ZONA, J.JUNTA, J.SEXO,
                                                 A. COD_JUNTA, A.COD_USUARIO, U.NOM_USUARIO, VOT_JUNTA,BLA_JUNTA,NUL_JUNTA       
                                                 FROM PROVINCIA P, CANTON C, PARROQUIA Q , ZONA Z, JUNTA J, ACTA A, USUARIO U
                                                 WHERE J.COD_ZONA=Z.COD_ZONA
                                                 AND Z.COD_PARROQUIA=Q.COD_PARROQUIA
                                                 AND J.COD_PARROQUIA=Q.COD_PARROQUIA
                                                 AND J.COD_CANTON=C.COD_CANTON
                                                 AND J.COD_PROVINCIA=P.COD_PROVINCIA
                                                 AND A.COD_JUNTA=J.COD_JUNTA
                                                 AND A.COD_USUARIO = U.COD_USUARIO(+)";
        
        private string consultaAsignacion = @" select cod_junta
                                         from acta 
                                         where cod_usuario = {0}";

        private string actualizaActa = @" update acta 
                                          set cod_usuario = {0}
                                         where cod_junta = {1}";
        public IEnumerable<ActaResponse> GetActas(int codigoProvincia)
        {
            List<ActaResponse> actas = null;

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
                        consultaActas = consultaActas + " AND J.COD_PROVINCIA = {0} ";
                        cmd.CommandText = string.Format(consultaActas, codigoProvincia);
                        cmd.BindByName = true;

                        OracleDataReader odr = cmd.ExecuteReader();

                        if (odr.HasRows)
                        {
                            actas = new List<ActaResponse>();
                            while (odr.Read())
                            {
                                ActaResponse acta = new ActaResponse
                                {
                                    USUARIO = Convert.ToString(odr["NOM_USUARIO"]),
                                    Cod_Canton = Convert.ToInt32(odr["COD_CANTON"]),
                                    Cod_Parroquia = Convert.ToInt32(odr["COD_PARROQUIA"]),
                                    Cod_Provincia = Convert.ToInt32(odr["COD_PROVINCIA"]),
                                    Cod_Zona = Convert.ToInt32(odr["COD_ZONA"]),
                                    junta = Convert.ToString(odr["JUNTA"]),
                                    Nom_Canton = Convert.ToString(odr["NOM_CANTON"]),
                                    Nom_Parroquia = Convert.ToString(odr["NOM_PARROQUIA"]),
                                    Nom_Provincia = Convert.ToString(odr["NOM_PROVINCIA"]),
                                    Nom_Zona = Convert.ToString(odr["NOM_ZONA"]),
                                    sexo = Convert.ToString(odr["SEXO"]),
                                    COD_JUNTA = Convert.ToInt32(odr["cod_junta"]),
                                    COD_USUARIO = Convert.ToInt32(odr["cod_usuario"]),
                                    BLA_JUNTA = Convert.ToInt32(odr["bla_junta"]),
                                    NUL_JUNTA = Convert.ToInt32(odr["nul_junta"]),
                                    VOT_JUNTA = Convert.ToInt32(odr["vot_junta"])
                                };
                                actas.Add(acta);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return actas;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return actas;
        }
        public ActaResponse GetActa(int junta)
        {
            ActaResponse acta = null;

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
                        consultaActas = consultaActas + " AND J.cod_junta = {0}";
                        cmd.CommandText = string.Format(consultaActas, junta);
                        cmd.BindByName = true;

                        OracleDataReader odr = cmd.ExecuteReader();

                        if (odr.HasRows)
                        {
                            while (odr.Read())
                            {
                                acta = new ActaResponse
                                {
                                    COD_JUNTA = Convert.ToInt32(odr["cod_junta"]),
                                    COD_USUARIO = Convert.ToInt32(odr["cod_usuario"]),
                                    BLA_JUNTA = Convert.ToInt32(odr["bla_junta"]),
                                    NUL_JUNTA = Convert.ToInt32(odr["nul_junta"]),
                                    VOT_JUNTA = Convert.ToInt32(odr["vot_junta"]),
                                    Nom_Canton = Convert.ToString(odr["NOM_CANTON"]),
                                    Nom_Parroquia = Convert.ToString(odr["NOM_PARROQUIA"]),
                                    Nom_Zona = Convert.ToString(odr["NOM_ZONA"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return acta = null;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return acta;
        }
        public int ActualizaActa(int cod_usuario, int junta)
        {
            int resultado = 0;

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
                        cmd.CommandText = string.Format(actualizaActa, cod_usuario, junta);
                        //cmd.BindByName = true;

                        return resultado = cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        return resultado;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }
            //return resultado;
        }

        public Acta ConsultarAsignacion(int codigoUsuario)
        {
            Acta acta = null;

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
                        cmd.CommandText = string.Format(consultaAsignacion, codigoUsuario);

                        OracleDataReader odr = cmd.ExecuteReader();

                        if (odr.HasRows)
                        {
                            while (odr.Read())
                            {
                                acta = new Acta
                                {
                                    COD_JUNTA = Convert.ToInt32(odr["cod_junta"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return acta = null;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return acta;
        }

        public int ActualizaAsignacion(int codigoJunta)
        {
            int resultado = 0;

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
                        cmd.CommandText = string.Format(actualizaActa, 0, codigoJunta);
                        //cmd.BindByName = true;

                        return resultado = cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        return resultado;
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
    }
}
