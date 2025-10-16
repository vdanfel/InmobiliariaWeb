using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Persona;
using Microsoft.Data.SqlClient;

namespace InmobiliariaWeb.Servicios
{
    public class AdendasService: IAdendasService
    {
        private readonly SqlConnection _connection;
        public AdendasService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> AdendasCreate(AdendasDTO adendasDTO)
        {
            int nIdent_Adendas = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_AdendasCreate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Contratos", adendasDTO.nIdent_Contratos);
                    command.Parameters.AddWithValue("@sTextoAdenda", adendasDTO.sTextoAdenda);
                    command.Parameters.AddWithValue("@nUsuarioCreacion", adendasDTO.nUsuarioCreacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        nIdent_Adendas = Int32.Parse(reader["nIdent_Adendas"].ToString());
                    }

                    return nIdent_Adendas;
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
        public async Task<AdendasDTO?> ObtenerAdendaPendiente(int nIdent_Contratos)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(@"
            SELECT TOP 1 nIdent_Adendas, nIdent_Contratos, sTextoAdenda, nIdent_023_EstadoAdenda
                         
            FROM Adendas
            WHERE nIdent_Contratos = @nIdent_Contratos
              AND nIdent_023_EstadoAdenda = 1143
            ORDER BY dFechaCreacion DESC", _connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@nIdent_Contratos", nIdent_Contratos);

                    await _connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        return new AdendasDTO
                        {
                            nIdent_Adendas = Convert.ToInt32(reader["nIdent_Adendas"]),
                            nIdent_Contratos = Convert.ToInt32(reader["nIdent_Contratos"]),
                            sTextoAdenda = reader["sTextoAdenda"].ToString(),
                            nIdent_023_EstadoAdenda = Convert.ToInt32(reader["nIdent_023_EstadoAdenda"]),
                        };
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<int> AdendasUpdate(AdendasDTO adendasDTO)
        {
            int res = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_AdendasUpdate", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Adendas", adendasDTO.nIdent_Adendas);
                    command.Parameters.AddWithValue("@nIdent_Contratos", (object?)adendasDTO.nIdent_Contratos ?? DBNull.Value);
                    command.Parameters.AddWithValue("@sTextoAdenda", (object?)adendasDTO.sTextoAdenda ?? DBNull.Value);
                    command.Parameters.AddWithValue("@nIdent_023_EstadoAdenda", (object?)adendasDTO.nIdent_023_EstadoAdenda ?? DBNull.Value);
                    command.Parameters.AddWithValue("@nUsuarioModificacion", adendasDTO.nUsuarioModificacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        res = Int32.Parse(reader["res"].ToString());
                    }

                    return res;
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
        public async Task<AdendasDTO> AdendasSelect(int nIdent_Adendas)
        {
            AdendasDTO adendasDTO = new AdendasDTO();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_AdendasSelect", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Adendas", nIdent_Adendas);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        adendasDTO.nIdent_Adendas = Int32.Parse(reader["nIdent_Adendas"].ToString());
                        adendasDTO.sTextoAdenda = reader["sTextoAdenda"].ToString();
                    }
                    return adendasDTO;
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
