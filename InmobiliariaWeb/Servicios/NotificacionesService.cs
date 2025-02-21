using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Notificaciones;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Notificaciones;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Result.Separaciones;
using Microsoft.Data.SqlClient;


namespace InmobiliariaWeb.Servicios
{
    public class NotificacionesService:INotificacionesService
    {
        private readonly SqlConnection _connection;
        public NotificacionesService(SqlConnection connection) 
        {
            _connection = connection;
        }
        public async Task<List<ProgramasCbxList>> ProgramaCbxListar()
        {
            var programasList = new List<ProgramasCbxList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Programa_CbxListar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var programas = new ProgramasCbxList();
                        programas.Ident_Programa = Int32.Parse(reader["IDENT_PROGRAMA"].ToString());
                        programas.Nombre_Programa = reader["NOMBRE_PROGRAMA"].ToString();
                        programasList.Add(programas);
                    }

                    return programasList;
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
        public async Task<List<NotificacionesList>> NotificacionesListar(IndexViewModel indexViewModel)
        {
            var notificacionesList = new List<NotificacionesList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Notificaciones_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Validar si los valores son null antes de agregar a los parámetros
                    command.Parameters.AddWithValue("@NombrePersona",
                        string.IsNullOrEmpty(indexViewModel.NombrePersona) ? (object)DBNull.Value : indexViewModel.NombrePersona);

                    // Si el Ident_Programa es 0 o un valor válido, asegurarse de que sea un número entero adecuado
                    command.Parameters.AddWithValue("@Ident_Programa",
                        indexViewModel.Ident_Programa == 0 ? (object)DBNull.Value : indexViewModel.Ident_Programa);

                    // Para Manzana, también validamos que no sea null
                    command.Parameters.AddWithValue("@Manzana",
                        string.IsNullOrEmpty(indexViewModel.Manzana) ? (object)DBNull.Value : indexViewModel.Manzana);

                    command.Parameters.AddWithValue("@Estado", indexViewModel.Estado);  // Estado ya está validado como no null

                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var notificaciones = new NotificacionesList();
                        notificaciones.Indice = Int32.Parse(reader["Indice"].ToString());
                        notificaciones.NombreCompleto = reader["NombreCompleto"].ToString();
                        notificaciones.NombrePrograma = reader["NombrePrograma"].ToString();
                        notificaciones.Manzana = reader["Manzana"].ToString();
                        notificaciones.Lote = Int32.Parse(reader["Lote"].ToString());
                        notificaciones.FechaPago = DateTime.Parse(reader["FechaPago"].ToString());
                        notificaciones.DiasMoras = Int32.Parse(reader["DiasMoras"].ToString());
                        notificaciones.MensajeWhatsApp = reader["MensajeWhatsApp"].ToString();
                        notificaciones.EstadoCliente = Int32.Parse(reader["EstadoCliente"].ToString());
                        notificaciones.Ident_Cuotas = Int32.Parse(reader["Ident_Cuotas"].ToString());
                        notificaciones.Ident_Persona = Int32.Parse(reader["Ident_Persona"].ToString());
                        notificacionesList.Add(notificaciones);
                    }
                    return notificacionesList;
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
        public async Task ConfirmarNotificacion(int Ident_Cuotas,int Ident_Persona, int Usuario)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_ClientesNotificados_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Cuotas", Ident_Cuotas);
                    command.Parameters.AddWithValue("@Ident_Persona", Ident_Persona);
                    command.Parameters.AddWithValue("@Usuario", Usuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
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
        public async Task<List<NotificacionesExportar>> ExportarExcel(string NombrePersona, int Ident_Programa, string Manzana, string Estado)
        {
            var notificacionesExportar = new List<NotificacionesExportar>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_notificacionesclientes_exportar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Validar si los valores son null antes de agregar a los parámetros
                    command.Parameters.AddWithValue("@NombrePersona",
                        string.IsNullOrEmpty(NombrePersona) ? (object)DBNull.Value : NombrePersona);

                    // Si el Ident_Programa es 0 o un valor válido, asegurarse de que sea un número entero adecuado
                    command.Parameters.AddWithValue("@Ident_Programa",
                        Ident_Programa == 0 ? (object)DBNull.Value : Ident_Programa);

                    // Para Manzana, también validamos que no sea null
                    command.Parameters.AddWithValue("@Manzana",
                        string.IsNullOrEmpty(Manzana) ? (object)DBNull.Value : Manzana);

                    command.Parameters.AddWithValue("@Estado", Estado);  // Estado ya está validado como no null

                    await _connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var notificaciones = new NotificacionesExportar();
                        notificaciones.Indice = Int32.Parse(reader["Indice"].ToString());
                        notificaciones.NombreCompleto = reader["NombreCompleto"].ToString();
                        notificaciones.NombrePrograma = reader["NombrePrograma"].ToString();
                        notificaciones.Manzana = reader["Manzana"].ToString();
                        notificaciones.Lote = Int32.Parse(reader["Lote"].ToString());
                        notificaciones.FechaPago = DateTime.Parse(reader["FechaPago"].ToString());
                        notificaciones.DiasMoras = Int32.Parse(reader["DiasMoras"].ToString());
                        notificaciones.EstadoCliente = Int32.Parse(reader["EstadoCliente"].ToString());
                        notificaciones.Datos = reader["Datos"].ToString();
                        notificaciones.CantidadNotificaciones = Int32.Parse(reader["CantidadNotificaciones"].ToString());

                        notificacionesExportar.Add(notificaciones);
                    }
                    return notificacionesExportar;
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
