using Dapper;
using Domain.Programa;
using Microsoft.Data.SqlClient;
using Repository.Interface.Programa;
using System.Data;

namespace Repository.Programa
{
    public class ProgramaRepository: IProgramaRepository
    {
        private readonly SqlConnection _connection;
        public ProgramaRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaOpciones()
        {
            var procedure = "usp_ProgramaOptions";
            try 
            {
                var res = await _connection.QueryAsync<ProgramaOpcionesDTO>(
                    procedure,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al culminar el contrato y actualizar el estado del lote.", ex);
            }
        }
        public async Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConCartaNotarial()
        {
            var procedure = "usp_ProgramaCartaNotarialOptions";
            try
            {
                var res = await _connection.QueryAsync<ProgramaOpcionesDTO>(
                    procedure,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al culminar el contrato y actualizar el estado del lote.", ex);
            }
        }
        public async Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConContrato()
        {
            var procedure = "usp_ProgramaContratoOptions";
            try
            {
                var res = await _connection.QueryAsync<ProgramaOpcionesDTO>(
                    procedure,
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
