using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Caja;
using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Models.Ingresos;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;
using Microsoft.Data.SqlClient;

namespace InmobiliariaWeb.Servicios
{
    public class CajaService:ICajaService
    {
        private readonly SqlConnection _connection;
        public CajaService(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Obtener_Ident_Ingresos(int Ident_021_TipoIngresos, int Ident_Origen)
        {
            int Ident_Ingresos = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("Ingresos_ObtenerIdent", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_021_TipoIngreso", Ident_021_TipoIngresos);
                    command.Parameters.AddWithValue("@Ident_Origen", Ident_Origen);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Ident_Ingresos = Int32.Parse(reader[0].ToString());
                    }
                    return Ident_Ingresos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<int> Ingresos_Insert(IngresosModel ingresosModel, LoginResult loginResult)
        {
            int Ident_Ingresos = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Ingresos_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_021_TipoIngresos", ingresosModel.Ident_021_TipoIngresos);
                    command.Parameters.AddWithValue("@Ident_Origen", ingresosModel.Ident_Origen);
                    command.Parameters.AddWithValue("@Ident_Programa", ingresosModel.Ident_Programa);
                    command.Parameters.AddWithValue("@Ident_Manzana", ingresosModel.Ident_Manzana);
                    command.Parameters.AddWithValue("@Ident_Lote", ingresosModel.Ident_Lote);
                    command.Parameters.AddWithValue("@FechaPago", ingresosModel.FechaPago);
                    command.Parameters.AddWithValue("@ImporteTotal", ingresosModel.ImporteTotal);
                    command.Parameters.AddWithValue("@Ident_002_TipoMoneda", ingresosModel.Ident_002_TipoMoneda);
                    command.Parameters.AddWithValue("@Observacion", ingresosModel.Observacion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Ident_015_EstadoPago", ingresosModel.Ident_015_EstadoPago);
                    command.Parameters.AddWithValue("@Ident_Persona", ingresosModel.Ident_Persona);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Ident_Ingresos = Int32.Parse(reader["Ident_Ingresos"].ToString());
                    }
                    return Ident_Ingresos;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task Ingresos_Update(IngresosModel ingresosModel, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Ingresos_Update", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Ingresos", ingresosModel.Ident_Ingresos);
                    command.Parameters.AddWithValue("@Ident_021_TipoIngresos", ingresosModel.Ident_021_TipoIngresos);
                    command.Parameters.AddWithValue("@Ident_Origen", ingresosModel.Ident_Origen);
                    command.Parameters.AddWithValue("@Ident_Programa", ingresosModel.Ident_Programa);
                    command.Parameters.AddWithValue("@Ident_Manzana", ingresosModel.Ident_Manzana);
                    command.Parameters.AddWithValue("@Ident_Lote", ingresosModel.Ident_Lote);
                    command.Parameters.AddWithValue("@FechaPago", ingresosModel.FechaPago);
                    command.Parameters.AddWithValue("@ImporteTotal", ingresosModel.ImporteTotal);
                    command.Parameters.AddWithValue("@Ident_002_TipoMoneda", ingresosModel.Ident_002_TipoMoneda);
                    command.Parameters.AddWithValue("@Observacion", ingresosModel.Observacion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Ident_015_EstadoPago", ingresosModel.Ident_015_EstadoPago);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<int> IngresosDetalle_Insert(IngresosDetalleModel ingresosDetalleModel,LoginResult loginResult)
        { 
            int Ident_IngresosDetalle = 0;
            
            try
            {
                using (SqlCommand command = new SqlCommand("usp_IngresosDetalle_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Ingresos", ingresosDetalleModel.Ident_Ingresos);
                    command.Parameters.AddWithValue("@Ident_018_TipoPago", ingresosDetalleModel.Ident_018_TipoPago);
                    command.Parameters.AddWithValue("@Ident_CuentasBancarias", ingresosDetalleModel.Ident_CuentasBancarias);
                    command.Parameters.AddWithValue("@Importe", ingresosDetalleModel.Importe);
                    command.Parameters.AddWithValue("@ImporteConTC", ingresosDetalleModel.ImporteConTC);
                    command.Parameters.AddWithValue("@NumeroOperacion", ingresosDetalleModel.NumeroOperacion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Ident_002_TipoMoneda", ingresosDetalleModel.Ident_002_TipoMoneda);
                    command.Parameters.AddWithValue("@TipoCambio", ingresosDetalleModel.TipoCambio);
                    command.Parameters.AddWithValue("@Fecha", ingresosDetalleModel.Fecha);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Ident_IngresosDetalle = Int32.Parse(reader["Ident_IngresosDetalle"].ToString());
                    }
                    return Ident_IngresosDetalle;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<List<IngresosDetallesList>> IngresosDetalle_List(int Ident_Ingresos)
        {
            List<IngresosDetallesList> ingresosDetallesList = new List<IngresosDetallesList>();

            try
            {
                using (SqlCommand command = new SqlCommand("usp_IngresoDetalle_List", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Ingresos", Ident_Ingresos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        IngresosDetallesList ingresosDetalles = new IngresosDetallesList();
                        ingresosDetalles.Indice = Int32.Parse(reader["Indice"].ToString());
                        ingresosDetalles.Ident_IngresosDetalle = Int32.Parse(reader["Ident_IngresosDetalle"].ToString());
                        ingresosDetalles.TipoPago = reader["TipoPago"].ToString();
                        ingresosDetalles.Importe = Decimal.Parse(reader["Importe"].ToString());
                        ingresosDetalles.NumeroOperacion = reader["NumeroOperacion"].ToString();
                        ingresosDetalles.Banco = reader["Banco"].ToString();
                        ingresosDetalles.NumeroCuenta = reader["NumeroCuenta"].ToString();
                        ingresosDetalles.Moneda = reader["Moneda"].ToString();
                        ingresosDetalles.Fecha = DateTime.Parse(reader["Fecha"].ToString());
                        ingresosDetalles.TipoCambio = Decimal.Parse(reader["TipoCambio"].ToString());
                        ingresosDetallesList.Add(ingresosDetalles);
                    }
                    return ingresosDetallesList;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task IngresosDetalle_Delete(int Ident_IngresosDetalle, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_IngresosDetalle_Delete", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_IngresosDetalle", Ident_IngresosDetalle);
                    command.Parameters.AddWithValue("@Usuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<List<CuentasBancariasList>> CuentasBancariasXBanco(int Ident_019_Banco)
        {
            List<CuentasBancariasList> cuentasBancariasList = new List<CuentasBancariasList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_CuentasBancarias_XBanco", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_019_Banco", Ident_019_Banco);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        CuentasBancariasList cuenta = new CuentasBancariasList();
                        cuenta.Ident_CuentasBancarias = Int32.Parse(reader["Ident_CuentasBancarias"].ToString());
                        cuenta.DetalleCuenta = reader["DetalleCuenta"].ToString();
                        cuenta.Ident_002_TipoMoneda = Int32.Parse(reader["Ident_002_TipoMoneda"].ToString());
                        cuentasBancariasList.Add(cuenta);
                    }
                    return cuentasBancariasList;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public bool TipoCambio_Existe(DateTime Fecha)
        {
            const string query = "SELECT COUNT(Ident_TipoCambio) FROM TipoCambio WHERE Ident_Contratos = @Fecha";

            try
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Fecha", Fecha);

                    if (_connection.State != System.Data.ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // Re-throwing the exception preserves the stack trace.
                throw new Exception("Error checking if FormatoVenta exists", ex);
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }
        public async Task<int> TipoCambio_Insert(TipoCambioModel tipoCambioModel)
        {
            int Ident_TipoCambio = 0;
            try 
            {
                return Ident_TipoCambio;
            }
            catch (Exception ex)
            {
                // Re-throwing the exception preserves the stack trace.
                throw new Exception("Error checking if FormatoVenta exists", ex);
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            
        }
        public async Task TipoCambio_Update(TipoCambioModel tipoCambioModel)
        {
            try 
            { 

            }
            catch (Exception ex)
            {
                // Re-throwing the exception preserves the stack trace.
                throw new Exception("Error checking if FormatoVenta exists", ex);
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }
        public async Task<decimal> IngresosDetalle_ImporteTotal(int Ident_Ingresos)
        { 
            decimal TotalImporte = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_IngresosDetalles_Totales", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Ingresos", Ident_Ingresos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        TotalImporte = decimal.Parse(reader["TotalImporte"].ToString());
                    }
                    return TotalImporte;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task Ingresos_ValidarImportes(int Ident_IngresosDetalle, int Ident_021_TipoIngresos)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Ingresos_ValidarImportes", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_IngresosDetalle", Ident_IngresosDetalle);
                    command.Parameters.AddWithValue("@Ident_021_TipoIngresos", Ident_021_TipoIngresos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex; // Considera usar un manejo de excepciones más detallado o un logger
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<List<IngresosIndexTablaDTO>> IngresosIndex(IngresosIndexFilterDTO ingresosIndexFilterDTO)
        {
            List<IngresosIndexTablaDTO> ingresosIndexTablaDTO = new List<IngresosIndexTablaDTO>();
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_IngresosIndex", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@dFechaDesde", ingresosIndexFilterDTO.dFechaDesde);
                    command.Parameters.AddWithValue("@dFechaHasta", ingresosIndexFilterDTO.dFechaHasta);
                    command.Parameters.AddWithValue("@nIdent_021_TipoIngresos", ingresosIndexFilterDTO.nIdent_021_TipoIngresos);
                    command.Parameters.AddWithValue("@nIdent_Programa", ingresosIndexFilterDTO.nIdent_Programa);
                    command.Parameters.AddWithValue("@nIdent_Manzana", ingresosIndexFilterDTO.nIdent_Manzana);
                    command.Parameters.AddWithValue("@nIdent_Lote", ingresosIndexFilterDTO.nIdent_Lote);
                    command.Parameters.AddWithValue("@nIdent_Persona", ingresosIndexFilterDTO.nIdent_Persona);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        IngresosIndexTablaDTO ingresos = new IngresosIndexTablaDTO();
                        ingresos.nIdent_Ingresos = Int32.Parse(reader["nIdent_Ingresos"].ToString());
                        ingresos.sPrograma = reader["sPrograma"].ToString();
                        ingresos.sManzana = reader["sManzana"].ToString();
                        ingresos.nLote = reader["nLote"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["nLote"]);
                        ingresos.sNombreCliente = reader["sNombreCliente"].ToString();
                        ingresos.sTipoIngreso = reader["sTipoIngreso"].ToString();
                        ingresos.sMoneda = reader["sMoneda"].ToString();
                        ingresos.nImporte = Decimal.Parse(reader["nImporte"].ToString());
                        ingresos.sTipoPago = reader["sTipoPago"].ToString();
                        ingresos.sNumeroOperacion = reader["sNumeroOperacion"].ToString();
                        ingresos.dFechaPago = DateTime.Parse(reader["dFechaPago"].ToString());
                        ingresos.nIdent_002_TipoMoneda = Int32.Parse(reader["nIdent_002_TipoMoneda"].ToString());
                        ingresosIndexTablaDTO.Add(ingresos);
                    }
                    return ingresosIndexTablaDTO;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<IngresosViewModel> IngresosSelect(int nIdent_Ingresos)
        {
            IngresosViewModel ingresosViewModel = new IngresosViewModel();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_IngresosSelect", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Ingresos", nIdent_Ingresos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        ingresosViewModel.nIdent_Ingresos = Int32.Parse(reader["nIdent_Ingresos"].ToString());
                        ingresosViewModel.nIdent_Persona = Int32.Parse(reader["nIdent_Persona"].ToString());
                        ingresosViewModel.sNombreCompleto = reader["sNombreCompleto"].ToString();
                        ingresosViewModel.sDocumento = reader["sDocumento"].ToString();
                        ingresosViewModel.nIdent_021_TipoIngresos = Int32.Parse(reader["nIdent_021_TipoIngresos"].ToString());
                        ingresosViewModel.dFechaIngreso = DateTime.Parse(reader["dFechaIngreso"].ToString());
                        ingresosViewModel.nIdent_Programa = Int32.Parse(reader["nIdent_Programa"].ToString());
                        ingresosViewModel.nIdent_Manzana = Int32.Parse(reader["nIdent_Manzana"].ToString());
                        ingresosViewModel.nIdent_Lote = Int32.Parse(reader["nIdent_Lote"].ToString());
                        ingresosViewModel.sObservacion = reader["sObservacion"].ToString();
                    }
                    return ingresosViewModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
