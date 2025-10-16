using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Kardex;
using InmobiliariaWeb.Models.Tablas;
using Microsoft.Data.SqlClient;

namespace InmobiliariaWeb.Servicios
{
    public class KardexService: IKardexService
    {
        private readonly SqlConnection _connection;
        private readonly decimal _penalidad;
        public KardexService(SqlConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _penalidad = decimal.Parse(configuration["Penalidad:Penalidad"]);
        }
        public async Task<KardexNuevoDTO> DatosAdenda(int nIdent_Contratos)
        {
            KardexNuevoDTO kardexNuevoDTO = new KardexNuevoDTO();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_KardexPorAdenda", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Contratos", nIdent_Contratos);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        kardexNuevoDTO.nIdent_Contratos = Int32.Parse(reader["nIdent_Contratos"].ToString());
                        kardexNuevoDTO.sNombrePrograma = reader["sNombrePrograma"].ToString();
                        kardexNuevoDTO.sManzana = reader["sManzana"].ToString();
                        kardexNuevoDTO.nLote = Int32.Parse(reader["nLote"].ToString());
                        kardexNuevoDTO.nSaldoPendiente = Decimal.Parse(reader["nSaldoPendiente"].ToString());
                        kardexNuevoDTO.nSaldoMorasPendientes = Decimal.Parse(reader["nSaldoMorasPendientes"].ToString());
                        kardexNuevoDTO.nTotalDeuda = Decimal.Parse(reader["nTotalDeuda"].ToString());
                    }

                    return kardexNuevoDTO;
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
        public async Task<int> KardexNuevoInsert(KardexNuevoDTO kardexNuevoDTO)
        {
            int nIdent_Kardex = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_KardexNuevoInsert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Contratos", kardexNuevoDTO.nIdent_Contratos);
                    command.Parameters.AddWithValue("@nIdent_Adendas", kardexNuevoDTO.nIdent_Adendas);
                    command.Parameters.AddWithValue("@nImporteTotal", kardexNuevoDTO.nImporteNuevo);
                    command.Parameters.AddWithValue("@nCantidadCuotas", kardexNuevoDTO.nCuotas);
                    command.Parameters.AddWithValue("@nImporteMensual", kardexNuevoDTO.nImporteMensual);
                    command.Parameters.AddWithValue("@nImporteFinal", kardexNuevoDTO.nImporteFinal);
                    command.Parameters.AddWithValue("@nPenalidad", _penalidad);
                    command.Parameters.AddWithValue("@dFechaInicio", kardexNuevoDTO.dFechaInicio);
                    command.Parameters.AddWithValue("@nDiaPago", kardexNuevoDTO.nDiaPago);
                    command.Parameters.AddWithValue("@nUsuarioCreacion", kardexNuevoDTO.nUsuarioCreacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        nIdent_Kardex = Int32.Parse(reader["nIdent_Kardex"].ToString());
                    }

                    return nIdent_Kardex;
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
