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
        private string sOperadoresDetalle = @"SELECT * FROM CONTEORAPIDO2021.DETALLEOPERADORES";
        private string sTransmitidasProvincia = @"SELECT * FROM CONTEORAPIDO2021.ACTASTRANSPROV";
        private string sTransmitidasCanton = @"SELECT * FROM CONTEORAPIDO2021.ACTASTRANSCANTON";
        private string sTransmitidasParroquia = @"SELECT * FROM CONTEORAPIDO2021.ACTASTRANSPARR";
        private string sTransmitidasDetalle = @"SELECT * FROM CONTEORAPIDO2021.DETALLETRANSMITIDAS";

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
                                    PCODIGO = Convert.ToInt32(odr["CODIGO"]),
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
        public async Task<IEnumerable<DetalleOperadores>> OperadoresDetalle(int? codigoParroquia = null)
        {
            List<DetalleOperadores> operadoresDetalles = null;

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

                        if (codigoParroquia.HasValue)
                            sOperadoresDetalle += " WHERE CODIGO = " + codigoParroquia.ToString();

                        cmd.CommandText = string.Format(sOperadoresDetalle);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            operadoresDetalles = new List<DetalleOperadores>();
                            while (odr.Read())
                            {
                                DetalleOperadores operador = new DetalleOperadores
                                {
                                    CODIGO = Convert.ToInt32(odr["CODIGO"]),
                                    PARROQUIA = Convert.ToString(odr["PARROQUIA"]),
                                    CCANTON = Convert.ToInt32(odr["CCANTON"]),
                                    CANTON = Convert.ToString(odr["CANTON"]),
                                    PROVINCIA = Convert.ToString(odr["PROVINCIA"]),
                                    JUNTA = Convert.ToInt32(odr["JUNTA"]),
                                    USUARIO = Convert.ToString(odr["USUARIO"]),
                                    TELEFONO = Convert.ToString(odr["TELEFONO"])
                                };
                                operadoresDetalles.Add(operador);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return operadoresDetalles;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return operadoresDetalles;
        }
        public async Task<IEnumerable<ATransmitidasProvincia>> TransmitidasProvincia(int? codigoProvincia = null)
        {
            List<ATransmitidasProvincia> transmitidasProvincia = null;

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
                            sTransmitidasProvincia += " WHERE CODIGO = " + codigoProvincia.ToString();

                        cmd.CommandText = string.Format(sTransmitidasProvincia);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            transmitidasProvincia = new List<ATransmitidasProvincia>();
                            while (odr.Read())
                            {
                                ATransmitidasProvincia tProvincia = new ATransmitidasProvincia
                                {
                                    CODIGO = Convert.ToInt32(odr["CODIGO"]),
                                    PROVINCIA = Convert.ToString(odr["PROVINCIA"]),
                                    JUNTAS = Convert.ToInt32(odr["JUNTAS"]),
                                    TRANSMITIDAS = Convert.ToInt32(odr["TRANSMITIDAS"])
                                };
                                transmitidasProvincia.Add(tProvincia);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return transmitidasProvincia;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return transmitidasProvincia;
        }
        public async Task<IEnumerable<ATransmitidasCanton>> TransmitidasCanton(int? codigoProvincia = null)
        {
            List<ATransmitidasCanton> transmitidasCanton = null;

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
                            sTransmitidasCanton += " WHERE CODIGO_PROVINCIA = " + codigoProvincia.ToString();

                        cmd.CommandText = string.Format(sTransmitidasCanton);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            transmitidasCanton = new List<ATransmitidasCanton>();
                            while (odr.Read())
                            {
                                ATransmitidasCanton tCanton = new ATransmitidasCanton
                                {
                                    CODIGO_PROVINCIA = Convert.ToInt32(odr["CODIGO_PROVINCIA"]),
                                    CODIGO = Convert.ToInt32(odr["CODIGO"]),
                                    PROVINCIA = Convert.ToString(odr["PROVINCIA"]),
                                    CANTON = Convert.ToString(odr["CANTON"]),
                                    JUNTAS = Convert.ToInt32(odr["JUNTAS"]),
                                    TRANSMITIDAS = Convert.ToInt32(odr["TRANSMITIDAS"])
                                };
                                transmitidasCanton.Add(tCanton);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return transmitidasCanton;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return transmitidasCanton;
        }
        public async Task<IEnumerable<ATransmitidasParroquias>> TransmitidasParroquia(int? codigoCanton = null)
        {
            List<ATransmitidasParroquias> transmitidasParroquia = null;

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
                            sTransmitidasParroquia += " WHERE CCANTON = " + codigoCanton.ToString();

                        cmd.CommandText = string.Format(sTransmitidasParroquia);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            transmitidasParroquia = new List<ATransmitidasParroquias>();
                            while (odr.Read())
                            {
                                ATransmitidasParroquias tParroquia = new ATransmitidasParroquias
                                {
                                    CCANTON = Convert.ToInt32(odr["CCANTON"]),
                                    PARROQUIA = Convert.ToString(odr["NOM_PARROQUIA"]),
                                    CODIGO = Convert.ToInt32(odr["CODIGO"]),
                                    PROVINCIA = Convert.ToString(odr["PROVINCIA"]),
                                    CANTON = Convert.ToString(odr["CANTON"]),
                                    JUNTAS = Convert.ToInt32(odr["JUNTAS"]),
                                    TRANSMITIDAS = Convert.ToInt32(odr["TRANSMITIDAS"])
                                };
                                transmitidasParroquia.Add(tParroquia);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return transmitidasParroquia;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return transmitidasParroquia;
        }
        public async Task<IEnumerable<DetallesTransmitidas>> TransmitidasDetalle(int? codigoParroquia = null)
        {
            List<DetallesTransmitidas> transmitidasDetalles = null;

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

                        if (codigoParroquia.HasValue)
                            sTransmitidasDetalle += " WHERE CODIGO = " + codigoParroquia.ToString();

                        cmd.CommandText = string.Format(sTransmitidasDetalle);

                        OracleDataReader odr = (OracleDataReader)await cmd.ExecuteReaderAsync();

                        if (odr.HasRows)
                        {
                            transmitidasDetalles = new List<DetallesTransmitidas>();
                            while (odr.Read())
                            {
                                DetallesTransmitidas transmitida = new DetallesTransmitidas
                                {
                                    CODIGO = Convert.ToInt32(odr["CODIGO"]),
                                    PARROQUIA = Convert.ToString(odr["PARROQUIA"]),
                                    CCANTON = Convert.ToInt32(odr["CCANTON"]),
                                    CANTON = Convert.ToString(odr["CANTON"]),
                                    PROVINCIA = Convert.ToString(odr["PROVINCIA"]),
                                    COD_JUNTA = Convert.ToInt32(odr["COD_JUNTA"]),
                                    JUNTA = Convert.ToInt32(odr["JUNTA"]),
                                    SEXO = Convert.ToString(odr["SEXO"]),
                                    USUARIO = Convert.ToString(odr["USUARIO"]),
                                    TELEFONO = Convert.ToString(odr["TELEFONO"]),
                                    TRANSMITIDAS = Convert.ToBoolean(odr["TRANSMITIDAS"])
                                };
                                transmitidasDetalles.Add(transmitida);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return transmitidasDetalles;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }

                }
            }

            return transmitidasDetalles;
        }
    }
}
