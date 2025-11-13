using Dapper;
using Domain.Manzana;
using Microsoft.Data.SqlClient;
using Repository.Interface.Manzana;
using System.Data;

namespace Repository.Manzana
{
    public class ManzanaRepository: IManzanaRepository
    {
        private readonly SqlConnection _connection;
        public ManzanaRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConContratoOpciones(int nIdent_Programa)
        {
            var procedure = "usp_ManzanaConContratoOptions";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Programa", nIdent_Programa);
                var res = await _connection.QueryAsync<ManzanaOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las manzanas.", ex);
            }
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConSeparacionesOpciones(int nIdent_Programa)
        {
            var procedure = "usp_ManzanaConSeparacionesOptions";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Programa", nIdent_Programa);
                var res = await _connection.QueryAsync<ManzanaOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las manzanas.", ex);
            }
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaLibreOpciones(int nIdent_Programa)
        {
            var procedure = "usp_ManzanasLibres";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Programa", nIdent_Programa);
                var res = await _connection.QueryAsync<ManzanaOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las manzanas.", ex);
            }
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConCartaNotarialOpciones(int nIdent_Programa)
        {
            var procedure = "usp_ManzanaConCartaNotarialOptions";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Programa", nIdent_Programa);
                var res = await _connection.QueryAsync<ManzanaOpcionesDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las manzanas.", ex);
            }
        }
    }
}
