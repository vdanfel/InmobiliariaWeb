using Domain.CartaNotarial;
using Domain.Contratos;
using Domain.Tablas;

namespace BusinessLogic.Interface.CartaNotarial
{
    public interface ICartaNotarialBL
    {
        Task<IEnumerable<CartaNotarialResponseDTO>> CartaNotarialBandeja(CartaNotarialRequestDTO cartaNotarialRequestDTO);
        Task<CartaNotarial1ViewDTO> CartaNotarialSelect(int nIdent_CartaNotarial);
        Task<IEnumerable<ClientesListCbxDTO>> CartaNotarialPersonaList(int nIdent_CartaNotarial);
        Task<IEnumerable<ItemCartaNotarialDetalleListDTO>> CartaNotarialDetalleList(int nIdent_CartaNotarial);
        Task<int> CartaNotarialCreate(CartaNotarialDTO cartaNotarialDTO);
        Task<int> CartaNotarialUpdate(CartaNotarialDTO cartaNotarialDTO);
        Task<int> CartaNotarialDetalleCreate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO);
        Task<int> CartaNotarialDetalleUpdate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO);
        Task<int> CartaNotarialPersonaCreate(CartaNotarialPersonaDTO cartaNotarialPersonaDTO);
        Task<int> CartaNotarialPersonaDelete(int nIdent_CartaNotarial, int nIdent_UsuarioModificacion);
        Task<ContratoPorLoteDTO> ObtenerContratoPorLote(int nIdent_Lote);
        Task<IEnumerable<ClientesListCbxDTO>> ListarClientesPorContrato(int nIdent_Contrato);
    }
}
