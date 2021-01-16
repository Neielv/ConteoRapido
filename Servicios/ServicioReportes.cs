using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.ViewModels.Reportes;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Servicios
{
    public class ServicioReportes : IServicioReportes
    {
        private readonly string _conn;

        public ServicioReportes(IConfiguration _configuration)
        {
            _conn = _configuration.GetConnectionString("OracleDBConnection");
        }

        private string sOperadoresProvincia = @"SELECT * FROM CONTEORAPIDO2021.ASIGOPERADORESPORPROVINCIA";
        private string sOperadoresCanton = @"SELECT * FROM CONTEORAPIDO2021.ASIGOPERADORESPORCANTON";
        private string sOperadoresParroquia = @"SELECT * FROM CONTEORAPIDO2021.ASIGOPERADORESPARROQUIA";

        public async Task<IEnumerable<AOperadoresProvincia>> OperadoresProvincia(int? codigoProvincia = null)
        {
            List<AOperadoresProvincia> operadoresProv = null;

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

                        if (codigoProvincia.HasValue)
                            sOperadoresProvincia += " WHERE CODIGO = " + codigoProvincia.ToString();

                        cmd.CommandText = string.Format(sOperadoresProvincia);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            operadoresProv = new List<AOperadoresProvincia>();
                            while (odr.Read())
                            {
                                AOperadoresProvincia operadorProvincia = new AOperadoresProvincia
                                {
                                    COD_PROV = Convert.ToInt32(odr["CODIGO"]),
                                    PROVINCIA = Convert.ToString(odr["PROVINCIA"]),
                                    JUNTAS = Convert.ToInt32(odr["JUNTAS"]),
                                    OPERADORES = Convert.ToInt32(odr["OPERADORES"])
                                };
                                operadoresProv.Add(operadorProvincia);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return operadoresProv;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return operadoresProv;
        }
        public async Task<IEnumerable<AOperadoresCanton>> OperadoresCanton(int? codigoProvincia = null)
        {
            List<AOperadoresCanton> operadoresCanton = null;

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

                        if (codigoProvincia.HasValue)
                            sOperadoresCanton += " WHERE CPROVINCIA = " + codigoProvincia.ToString();

                        cmd.CommandText = string.Format(sOperadoresCanton);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            operadoresCanton = new List<AOperadoresCanton>();
                            while (odr.Read())
                            {
                                AOperadoresCanton operadorCanton = new AOperadoresCanton
                                {
                                    COD_CANTON = Convert.ToInt32(odr["CODIGO"]),
                                    CANTON = Convert.ToString(odr["CANTONES"]),
                                    operadoresProvincia = new AOperadoresProvincia()
                                    {
                                        COD_PROV = Convert.ToInt32(odr["CPROVINCIA"]),
                                        PROVINCIA = Convert.ToString(odr["PROVINCIAS"]),
                                        JUNTAS = Convert.ToInt32(odr["JUNTAS"]),
                                        OPERADORES = Convert.ToInt32(odr["OPERADORES"])
                                    }
                                };
                                operadoresCanton.Add(operadorCanton);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return operadoresCanton;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return operadoresCanton;
        }
        public async Task<IEnumerable<AOperadoresParroquia>> OperadoresParroquia(int? codigoCanton = null)
        {
            List<AOperadoresParroquia> operadoresParroquias = null;

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

                        if (codigoCanton.HasValue)
                            sOperadoresParroquia += " WHERE CCANTON = " + codigoCanton.ToString();

                        cmd.CommandText = string.Format(sOperadoresParroquia);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            operadoresParroquias = new List<AOperadoresParroquia>();
                            while (odr.Read())
                            {
                                AOperadoresParroquia operadorParroquia = new AOperadoresParroquia
                                {
                                    PARROQUIA = Convert.ToString(odr["NOM_PARROQUIA"]),
                                    operadoresCanton = new AOperadoresCanton()
                                    {
                                        COD_CANTON = Convert.ToInt32(odr["CCANTON"]),
                                        CANTON = Convert.ToString(odr["CANTON"]),
                                        operadoresProvincia = new AOperadoresProvincia()
                                        {
                                            PROVINCIA = Convert.ToString(odr["PROVINCIA"]),
                                            JUNTAS = Convert.ToInt32(odr["JUNTAS"]),
                                            OPERADORES = Convert.ToInt32(odr["OPERADORES"])
                                        }
                                    }
                                };
                                operadoresParroquias.Add(operadorParroquia);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return operadoresParroquias;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return operadoresParroquias;
        }
    }
}
