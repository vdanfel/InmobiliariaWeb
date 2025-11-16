using Dapper;
using Domain.CartaNotarial;
using Domain.Contratos;
using Domain.Tablas;
using Microsoft.Data.SqlClient;
using Repository.Interface.CartaNotarial;
using System.Data;

namespace Repository.CartaNotarial
{
    public class CartaNotarialRepository: ICartaNotarialRepository
    {
        private readonly SqlConnection _connection;
        public CartaNotarialRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<CartaNotarialResponseDTO>> CartaNotarialBandeja(CartaNotarialRequestDTO cartaNotarialRequestDTO)
        {
            var procedure = "usp_CartaNotarialBandeja";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Programa", cartaNotarialRequestDTO.nIdent_Programa);
                parameters.Add("@nIdent_Manzana", cartaNotarialRequestDTO.nIdent_Manzana);
                parameters.Add("@nIdent_Lote", cartaNotarialRequestDTO.nIdent_Lote);
                parameters.Add("@sBuscar", cartaNotarialRequestDTO.sBuscar);

                var res = await _connection.QueryAsync<CartaNotarialResponseDTO>(
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
        public async Task<CartaNotarial1ViewDTO> CartaNotarialSelect(int nIdent_CartaNotarial)
        {
            var procedure = "usp_CartaNotarialSelect";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarial", nIdent_CartaNotarial);
                var res = await _connection.QueryFirstOrDefaultAsync<CartaNotarial1ViewDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al seleccionar datos de la carta notarial.", ex);
            }
        }
        public async Task<IEnumerable<ClientesListCbxDTO>> CartaNotarialPersonaList(int nIdent_CartaNotarial)
        {
            var procedure = "usp_CartaNotarialPersonaList";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarial", nIdent_CartaNotarial);
                var res = await _connection.QueryAsync<ClientesListCbxDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar clientes de la carta notarial.", ex);
            }
        }
        public async Task<IEnumerable<ItemCartaNotarialDetalleListDTO>>CartaNotarialDetalleList(int nIdent_CartaNotarial)
        {
            var procedure = "usp_CartaNotarialDetalleList";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarial", nIdent_CartaNotarial);
                var res = await _connection.QueryAsync<ItemCartaNotarialDetalleListDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar detalles de la carta notarial.", ex);
            }
        }
        public async Task<int> CartaNotarialCreate(CartaNotarialDTO cartaNotarialDTO)
        {
            var procedure = "usp_CartaNotarialCreate";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Contratos", cartaNotarialDTO.nIdent_Contratos);
                parameters.Add("@dFechaCartaNotarial", cartaNotarialDTO.dFechaCartaNotarial);
                parameters.Add("@nIdent_027_TipoCartaNotarial", cartaNotarialDTO.nIdent_027_TipoCartaNotarial);
                parameters.Add("@nIdent_CartaNotarialOrigen", cartaNotarialDTO.nIdent_CartaNotarialOrigen);
                parameters.Add("@nIdent_UsuarioCreacion", cartaNotarialDTO.nIdent_UsuarioCreacion);
                var nIdent_CartaNotarial = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return nIdent_CartaNotarial;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crear carta notarial .", ex);
            }
        }
        public async Task<int> CartaNotarialUpdate(CartaNotarialDTO cartaNotarialDTO)
        {
            var procedure = "usp_CartaNotarialUpdate";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarial", cartaNotarialDTO.nIdent_CartaNotarial);
                parameters.Add("@dFechaCartaNotarial", cartaNotarialDTO.dFechaCartaNotarial);
                parameters.Add("@nIdent_UsuarioModificacion", cartaNotarialDTO.nIdent_UsuarioModificacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error actualizar carta notarial .", ex);
            }
        }
        public async Task<int> CartaNotarialDetalleCreate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO)
        {
            var procedure = "usp_CartaNotarialDetalleCreate";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarial", cartaNotarialDetalleDTO.nIdent_CartaNotarial);
                parameters.Add("@nIdent_026_EstadoCartaNotarial", cartaNotarialDetalleDTO.nIdent_026_EstadoCartaNotarial);
                parameters.Add("@sObservacion",cartaNotarialDetalleDTO.sObservacion);
                parameters.Add("@nIdent_UsuarioCreacion", cartaNotarialDetalleDTO.nIdent_UsuarioCreacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crear detalle de carta notarial .", ex);
            }
        }
        public async Task<int> CartaNotarialDetalleUpdate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO)
        {
            var procedure = "usp_CartaNotarialDetalleUpdate";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarialDetalle", cartaNotarialDetalleDTO.nIdent_CartaNotarialDetalle);
                parameters.Add("@sObservacion", cartaNotarialDetalleDTO.sObservacion);
                parameters.Add("@bActivo", cartaNotarialDetalleDTO.bActivo);
                parameters.Add("@nIdent_UsuarioModificacion", cartaNotarialDetalleDTO.nIdent_UsuarioModificacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error actualizar detalle de carta notarial .", ex);
            }
        }
        public async Task<int> CartaNotarialPersonaCreate(CartaNotarialPersonaDTO cartaNotarialPersonaDTO)
        {
            var procedure = "usp_CartaNotarialPersonaCreate";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarial", cartaNotarialPersonaDTO.nIdent_CartaNotarial);
                parameters.Add("@nIdent_Persona", cartaNotarialPersonaDTO.nIdent_Persona);
                parameters.Add("@nIdent_UsuarioCreacion", cartaNotarialPersonaDTO.nIdent_UsuarioCreacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crear persona de carta notarial .", ex);
            }
        }
        public async Task<int> CartaNotarialPersonaDelete(int nIdent_CartaNotarial, int nIdent_UsuarioModificacion)
        {
            var procedure = "usp_CartaNotarialPersonaDelete";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_CartaNotarial", nIdent_CartaNotarial);
                parameters.Add("@nIdent_UsuarioModificacion", nIdent_UsuarioModificacion);
                var res = await _connection.QueryFirstOrDefaultAsync<int>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error eliminar persona de carta notarial .", ex);
            }
        }
        public async Task<ContratoPorLoteDTO> ObtenerContratoPorLote(int nIdent_Lote)
        {
            var procedure = "usp_ContratoPorLote_Obtener";
            try 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Lote", nIdent_Lote);
                var res = await _connection.QueryFirstOrDefaultAsync<ContratoPorLoteDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener contratos con carta notarial .", ex);
            }
        }
        public async Task<IEnumerable<ClientesListCbxDTO>> ListarClientesPorContrato(int nIdent_Contrato)
        {
            var procedure = "usp_ClientesPorContrato_Listar";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nIdent_Contrato", nIdent_Contrato);
                var res = await _connection.QueryAsync<ClientesListCbxDTO>(
                    procedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar clientes por contrato.", ex);
            }
        }
    }
}
