using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.CartaNotarial;
using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Separaciones;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InmobiliariaWeb.Servicios
{
    public class CartaNotarialService : ICartaNotarialService
    {
        private readonly SqlConnection _connection;
        public CartaNotarialService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa)
        {
            var manzanaCbxLists = new List<ManzanaCbxList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Manzana_CbxListarContrato", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", ident_Programa);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var manzanas = new ManzanaCbxList();
                        manzanas.Ident_Manzana = Int32.Parse(reader["IDENT_MANZANA"].ToString());
                        manzanas.Letra = reader["LETRA"].ToString();
                        manzanaCbxLists.Add(manzanas);
                    }
                    return manzanaCbxLists;
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
        public async Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana)
        {
            var lotesCbxList = new List<LoteCbxList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Lote_CbxListarContrato", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Manzana", ident_Manzana);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var lote = new LoteCbxList();
                        lote.Ident_Lote = Int32.Parse(reader["IDENT_LOTE"].ToString());
                        lote.Correlativo = Int32.Parse(reader["CORRELATIVO"].ToString());
                        lotesCbxList.Add(lote);
                    }
                    return lotesCbxList;
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
        public async Task<ContratosDTO> ObtenerContratoPorLote(int nIdent_Lote)
        {
            ContratosDTO contrato = null;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ContratoPorLote_Obtener", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Lote", nIdent_Lote);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        contrato = new ContratosDTO
                        {
                            nIdent_Contratos = Convert.ToInt32(reader["nIdent_Contratos"]),
                            sNumeroContrato = reader["sNumeroContrato"].ToString(),
                            nCuotasPendientes = reader["nCuotasPendientes"].ToString()
                        };
                    }

                    return contrato;
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
        public async Task<List<ClientesListCbxDTO>> ListarClientesPorContrato(int nIdent_Contrato)
        {
            var clientes = new List<ClientesListCbxDTO>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ClientesPorContrato_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Contrato", nIdent_Contrato);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var cliente = new ClientesListCbxDTO
                        {
                            nIdent_Persona = Convert.ToInt32(reader["nIdent_Persona"]),
                            sNombreCompleto = reader["sNombreCompleto"].ToString()
                        };
                        clientes.Add(cliente);
                    }

                    return clientes;
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
        public async Task<int> CartaNotarialCreate(CartaNotarialDTO cartaNotarialDTO)
        {
            var nIdent_CartaNotarial = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CartaNotarialCreate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Contratos", cartaNotarialDTO.nIdent_Contratos);
                    command.Parameters.AddWithValue("@dFechaCartaNotarial", cartaNotarialDTO.dFechaCartaNotarial);
                    command.Parameters.AddWithValue("@nIdent_027_TipoCartaNotarial", cartaNotarialDTO.nIdent_027_TipoCartaNotarial);
                    command.Parameters.AddWithValue("@nIdent_CartaNotarialOrigen", (object?)cartaNotarialDTO.nIdent_CartaNotarialOrigen ?? DBNull.Value);
                    command.Parameters.AddWithValue("@nIdent_UsuarioCreacion", cartaNotarialDTO.nIdent_UsuarioCreacion);
                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        nIdent_CartaNotarial = Int32.Parse(reader["nIdent_CartaNotarial"].ToString());
                    }
                    return nIdent_CartaNotarial;
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
        public async Task<int> CartaNotarialUpdate(CartaNotarialDTO cartaNotarialDTO)
        {
            var result = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CartaNotarialUpdate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_CartaNotarial", cartaNotarialDTO.nIdent_CartaNotarial);
                    command.Parameters.AddWithValue("@dFechaCartaNotarial", cartaNotarialDTO.dFechaCartaNotarial);
                    command.Parameters.AddWithValue("@nIdent_UsuarioModificacion", cartaNotarialDTO.nIdent_UsuarioModificacion);
                    await _connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result = Int32.Parse(reader["res"].ToString());
                    }
                    return result;
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
        public async Task<int> CartaNotarialDetalleCreate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO)
        {
            var nIdent_CartaNotarialDetalle = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CartaNotarialDetalleCreate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_CartaNotarial", cartaNotarialDetalleDTO.nIdent_CartaNotarial);
                    command.Parameters.AddWithValue("@nIdent_026_EstadoCartaNotarial", cartaNotarialDetalleDTO.nIdent_026_EstadoCartaNotarial);
                    command.Parameters.AddWithValue("@sObservacion",
                    string.IsNullOrEmpty(cartaNotarialDetalleDTO.sObservacion)
                        ? DBNull.Value
                        : (object)cartaNotarialDetalleDTO.sObservacion);
                    command.Parameters.AddWithValue("@nIdent_UsuarioCreacion", cartaNotarialDetalleDTO.nIdent_UsuarioCreacion);
                    await _connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        nIdent_CartaNotarialDetalle = Int32.Parse(reader["nIdent_CartaNotarialDetalle"].ToString());
                    }
                    return nIdent_CartaNotarialDetalle;
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
        public async Task<int> CartaNotarialDetalleUpdate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO)
        {
            var result = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CartaNotarialDetalleUpdate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_CartaNotarialDetalle", cartaNotarialDetalleDTO.nIdent_CartaNotarialDetalle);
                    command.Parameters.AddWithValue("@sObservacion", cartaNotarialDetalleDTO.sObservacion);
                    command.Parameters.AddWithValue("@bActivo", cartaNotarialDetalleDTO.bActivo);
                    command.Parameters.AddWithValue("@nIdent_UsuarioModificacion", cartaNotarialDetalleDTO.nIdent_UsuarioModificacion);
                    await _connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result = Int32.Parse(reader["res"].ToString());
                    }
                    return result;
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
        public async Task<int> CartaNotarialPersonaCreate(CartaNotarialPersonaDTO cartaNotarialPersonaDTO)
        {
            var nIdent_CartaNotarialPersona = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CartaNotarialPersonaCreate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_CartaNotarial", cartaNotarialPersonaDTO.nIdent_CartaNotarial);
                    command.Parameters.AddWithValue("@nIdent_Persona", cartaNotarialPersonaDTO.nIdent_Persona);
                    command.Parameters.AddWithValue("@nIdent_UsuarioCreacion", cartaNotarialPersonaDTO.nIdent_UsuarioCreacion);
                    await _connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        nIdent_CartaNotarialPersona = Int32.Parse(reader["nIdent_CartaNotarialPersona"].ToString());
                    }
                    return nIdent_CartaNotarialPersona;
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
        public async Task<int> CartaNotarialPersonaDelete(int nIdent_CartaNotarial, int nIdent_UsuarioModificacion)
        {
            var result = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CartaNotarialPersonaDelete", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_CartaNotarial", nIdent_CartaNotarial);
                    command.Parameters.AddWithValue("@nIdent_UsuarioModificacion", nIdent_UsuarioModificacion);
                    await _connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result = Int32.Parse(reader["res"].ToString());
                    }
                    return result;
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
        public async Task<CartaNotarialDTO> CartaNotarialSelect(int nIdent_CartaNotarial)
        {
            CartaNotarialDTO cartaNotarialDTO = null;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CartaNotarialSelect", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_CartaNotarial", nIdent_CartaNotarial);
                    await _connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        cartaNotarialDTO = new CartaNotarialDTO
                        {
                            nIdent_CartaNotarial = Convert.ToInt32(reader["nIdent_CartaNotarial"]),
                            nIdent_Contratos = Convert.ToInt32(reader["nIdent_Contratos"]),
                            nIdent_027_TipoCartaNotarial = Convert.ToInt32(reader["nIdent_027_TipoCartaNotarial"]),
                            nIdent_026_EstadoCartaNotarial = Convert.ToInt32(reader["nIdent_026_EstadoCartaNotarial"]),
                            sSerie = reader["sSerie"].ToString(),
                            nCorrelativo = Convert.ToInt32(reader["nCorrelativo"]),
                            dFechaCartaNotarial = Convert.ToDateTime(reader["dFechaCartaNotarial"]),
                            sNombreNotaria = reader["sNombreNotaria"].ToString(),
                            nIdent_CartaNotarialOrigen = reader["nIdent_CartaNotarialOrigen"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["nIdent_CartaNotarialOrigen"]),
                        };
                    }
                    return cartaNotarialDTO;
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
    }
}
