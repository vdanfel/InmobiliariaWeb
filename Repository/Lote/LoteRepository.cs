using Dapper;
using Domain.Lote;
using Microsoft.Data.SqlClient;
using Repository.Interface.Lote;
using System.Data;

namespace Repository.Lote
{
    public class LoteRepository: ILoteRepository
    {
        private readonly SqlConnection _connection;

        public LoteRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteLibreOpciones(int nIdent_Manzana)
        {
            var procedure = "usp_LoteLibreOptions";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Manzana", nIdent_Manzana);
                var res = await _connection.QueryAsync<LoteOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los lotes.", ex);
            }
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteConContratoOpciones(int nIdent_Manzana)
        {
            var procedure = "usp_LoteConContratoOptions";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Manzana", nIdent_Manzana);
                var res = await _connection.QueryAsync<LoteOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los lotes.", ex);
            }
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteConSeparacionOpciones(int nIdent_Manzana)
        {
            var procedure = "usp_LoteConSeparacionOptions";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Manzana", nIdent_Manzana);
                var res = await _connection.QueryAsync<LoteOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los lotes.", ex);
            }
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteConCartaNotarialOpciones(int nIdent_Manzana)
        {
            var procedure = "usp_LoteConCartaNotarialOptions";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Manzana", nIdent_Manzana);
                var res = await _connection.QueryAsync<LoteOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los lotes.", ex);
            }
        }
    }
}
