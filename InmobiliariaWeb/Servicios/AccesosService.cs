using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Result;
using Microsoft.Data.SqlClient;

namespace InmobiliariaWeb.Servicios
{
    public class AccesosService : IAccesosService
    {
        private readonly SqlConnection _connection;
        public AccesosService(SqlConnection connection)
        { 
            _connection = connection;
        }
        public async Task<AccesoResult> ValidarAcceso(int IdentUsuario, int IdentPagina)
        {
            AccesoResult accesoResult = new AccesoResult();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Accesos_Usuario", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Usuario", IdentUsuario);
                    command.Parameters.AddWithValue("@ISIdent_Paginas", IdentPagina);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        accesoResult.Visualizar = Int32.Parse(reader["VISUALIZAR"].ToString());
                        accesoResult.Lectura = Int32.Parse(reader["LECTURA"].ToString());
                        accesoResult.Escritura = Int32.Parse(reader["ESCRITURA"].ToString());
                    }
                    return accesoResult;
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
