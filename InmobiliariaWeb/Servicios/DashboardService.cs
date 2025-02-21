using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Result.Dashboard;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Result.Separaciones;
using Microsoft.Data.SqlClient;

namespace InmobiliariaWeb.Servicios
{
    public class DashboardService:IDashboardService
    {
        private readonly SqlConnection _connection;
        public DashboardService(SqlConnection connection)
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
        public async Task<List<TotalesProgramas>> TTotalesProgramas(int Ident_Programa, int Anio, int Mes)
        { 
            List<TotalesProgramas> gG1Lists = new List<TotalesProgramas>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Dashboard_TotalesProgramas", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Programa ", Ident_Programa);
                    command.Parameters.AddWithValue("@Anio ", Anio);
                    command.Parameters.AddWithValue("@Mes ", Mes);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        TotalesProgramas gG1List = new TotalesProgramas();
                        gG1List.Ident_Programa = Int32.Parse(reader["Ident_Programa"].ToString());
                        gG1List.NombrePrograma = reader["NombrePrograma"].ToString();
                        gG1List.Conteo_Lotes = reader["Conteo_Lotes"].ToString();
                        gG1List.Pagos_Realizados = reader["Pagos_Realizados"].ToString();
                        gG1List.Importes_Pagados = reader["Importes_Pagados"].ToString();
                        gG1Lists.Add(gG1List);
                    }

                    return gG1Lists;
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
        public async Task<VSPeriodos> TVSPeriodos(int Ident_Programa, int Anio, int Mes)
        { 
            var vsPeriodos = new VSPeriodos();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Dashboard_VSPeriodos", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ident_Programa ", Ident_Programa);
                    command.Parameters.AddWithValue("@Anio ", Anio);
                    command.Parameters.AddWithValue("@Mes ", Mes);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        vsPeriodos.AnioAnterior = Int32.Parse(reader["AnioAnterior"].ToString());
                        vsPeriodos.AnioActual = Int32.Parse(reader["AnioActual"].ToString());
                        vsPeriodos.TotalCuotaAnterior = decimal.Parse(reader["TotalCuotaAnterior"].ToString());
                        vsPeriodos.TotalCuotaActual = decimal.Parse(reader["TotalCuotaActual"].ToString());
                        vsPeriodos.TotalPagadoAnterior = decimal.Parse(reader["TotalPagadoAnterior"].ToString());
                        vsPeriodos.TotalPagadoActual = decimal.Parse(reader["TotalPagadoActual"].ToString());
                    }

                    return vsPeriodos;
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
