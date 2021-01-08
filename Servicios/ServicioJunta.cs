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
    public class ServicioJunta : IServicioJunta
    {

        private readonly string _conn;
        public ServicioJunta(IConfiguration _configuration)
        {
            _conn = _configuration.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<Junta> ObtenerJuntas()
        {

            List<Junta> listaJuntas = new List<Junta>();

            using (OracleConnection con = new OracleConnection(_conn))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = "select * from junta where cod_provincia = 6";
                        OracleDataReader odr = cmd.ExecuteReader();
                        //odr.Read();
                        while (odr.Read())
                        {
                            Junta junta = new Junta
                            {
                                COD_PROVINCIA = Convert.ToInt32(odr["cod_provincia"]),
                                COD_CANTON = Convert.ToInt32(odr["cod_canton"]),
                                COD_PARROQUIA = Convert.ToInt32(odr["cod_parroquia"]),
                                COD_JUNTA = Convert.ToInt32(odr["cod_junta"]),
                                SEXO = Convert.ToChar(odr["sexo"])
                            };
                            listaJuntas.Add(junta);
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

            return listaJuntas;
        }

        //public object ObtieneJunta(ConsultaJunta consulta)
        //{

        //}
        public Junta GetActa(int iUsuario, int iEstado)
        {
            Junta acta = new Junta();

            using (OracleConnection con = new OracleConnection(_conn))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PKG_CONTEO_RAPIDO.CONSULTA_ACTA_POR_USUARIO";
                        cmd.BindByName = true;

                        cmd.Parameters.Add("Return_Value", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
                        cmd.Parameters.Add("i_usuario", OracleDbType.Int32, iUsuario, ParameterDirection.Input);
                        cmd.Parameters.Add("i_estado", OracleDbType.Int32, iEstado, ParameterDirection.Input);
                        cmd.Parameters.Add("o_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                        OracleDataReader odr = cmd.ExecuteReader();
                        
                        while (odr.Read())
                        {
                            acta.COD_CANTON = Convert.ToInt32(odr["cod_canton"]);
                            acta.COD_JUNTA = Convert.ToInt32(odr["cod_junta"]);
                            acta.COD_PROVINCIA = Convert.ToInt32(odr["cod_provincia"]);
                            acta.COD_PARROQUIA = Convert.ToInt32(odr["cod_parroquia"]);
                            //acta.COD_USUARIO = Convert.ToInt32(odr["cod_usuario"]);
                            acta.COD_ZONA = Convert.ToInt32(odr["cod_zona"]);
                            acta.JUNTA = Convert.ToInt32(odr["junta"]);
                            //acta.VOT_JUNTA = Convert.ToInt32(odr["vot_junta"]);
                            //acta.BLA_JUNTA = Convert.ToInt32(odr["bla_junta"]);
                            //acta.NUL_JUNTA = Convert.ToInt32(odr["nul_junta"]);
                            //acta.NOM_PROVINCIA = Convert.ToString(odr["nom_provincia"]);
                            //acta.NOM_CANTON = Convert.ToString(odr["nom_canto"]);
                            //acta.NOM_PARROQUIA = Convert.ToString(odr["nom_parroquia"]);
                            //acta.NOM_ZONA = Convert.ToString(odr["nom_zona"]);
                            acta.SEXO = Convert.ToChar(odr["sexo"]);
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

            return acta;
        }

        
    }
}
