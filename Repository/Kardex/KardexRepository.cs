using Dapper;
using Domain.Adendas;
using Microsoft.Data.SqlClient;
using Repository.Interface.Kardex;
using System.Data;

namespace Repository.Kardex
{
    public class KardexRepository : IKardexRepository
    {
        private readonly SqlConnection _connection;
        public KardexRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> KardexReprogramacion(ValoresReprogramacionDTO valoresReprogramacionDTO)
        {
            var procedure = "usp_KardexReprogramacion";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Kardex", valoresReprogramacionDTO.nIdent_Kardex);
                parameters.Add("@dNuevaFechaInicio", valoresReprogramacionDTO.dNuevaFechaInicio);
                parameters.Add("@nNuevoDiaPago", valoresReprogramacionDTO.nNuevoDiaPago);
                parameters.Add("@nUsuarioModificacion", valoresReprogramacionDTO.nUsuarioModificacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al hacer la reprogramacion de cuotas.", ex);
            }
        }
    }
}
