using Dapper;
using Microsoft.Data.SqlClient;
using Repository.Interface.Cuota;
using System.Data;

namespace Repository.Cuota
{
    public class CuotaRepository: ICuotaRepository
    {
        private readonly SqlConnection _connection;
        public CuotaRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> CuotasCompletas(int nIdent_Kardex)
        {
            var procedure = "usp_PagosCompletos";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Kardex", nIdent_Kardex);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al culminar el contrato y actualizar el estado del lote.", ex);
            }
        }
    }
}
