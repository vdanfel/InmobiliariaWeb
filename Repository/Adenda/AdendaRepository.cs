using Dapper;
using Domain.Tablas;
using Microsoft.Data.SqlClient;
using Repository.Interface.Adenda;
using System.Data;

namespace Repository.Adenda
{
    public class AdendaRepository: IAdendaRepository
    {
        private readonly SqlConnection _connection;
        public AdendaRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> AdendasCreate(AdendasDTO adendasDTO)
        {
            var procedure = "usp_AdendasCreate";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Contratos", adendasDTO.nIdent_Contratos);
                parameters.Add("@nIdent_028_TipoAdenda", adendasDTO.nIdent_028_TipoAdenda);
                parameters.Add("@sTextoAdenda", adendasDTO.sTextoAdenda);
                parameters.Add("@nUsuarioCreacion", adendasDTO.nUsuarioCreacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crear adenda .", ex);
            }

        }
        public async Task<AdendasDTO> ObtenerAdendaPendiente(AdendasDTO adendasDTO)
        {
            var procedure = "usp_AdendaPendiente";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Contratos", adendasDTO.nIdent_Contratos);
                parameters.Add("@nIdent_028_TipoAdenda", adendasDTO.nIdent_028_TipoAdenda);
                var res = await _connection.QueryFirstOrDefaultAsync<AdendasDTO>(
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
        public async Task<int> AdendasUpdate(AdendasDTO adendasDTO)
        {
            var procedure = "usp_AdendasUpdate";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Adendas", adendasDTO.nIdent_Adendas);
                parameters.Add("@nIdent_Contratos", adendasDTO.nIdent_Contratos);
                parameters.Add("@sTextoAdenda", adendasDTO.sTextoAdenda);
                parameters.Add("@nIdent_023_EstadoAdenda", adendasDTO.nIdent_023_EstadoAdenda);
                parameters.Add("@nUsuarioModificacion", adendasDTO.nUsuarioModificacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error actualizar adenda .", ex);
            }
        }
        public async Task<AdendasDTO> AdendasSelect(int nIdent_Adendas)
        {
            var procedure = "usp_AdendasSelect";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Adendas", nIdent_Adendas);
                var res = await _connection.QueryFirstOrDefaultAsync<AdendasDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error seleccionar adenda .", ex);
            }
        }
    }
}
