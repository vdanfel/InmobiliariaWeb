using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.CartaNotarial;
using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Separaciones;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InmobiliariaWeb.Servicios
{
    public class CartaNotarialService: ICartaNotarialService
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
                            sNumeroContrato = reader["sNumeroContrato"].ToString()
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
    }
}
