using Domain.CartaNotarial;

namespace Repository.Interface.CartaNotarial
{
    public interface ICartaNotarialRepository
    {
        Task<IEnumerable<CartaNotarialResponseDTO>> CartaNotarialBandeja(CartaNotarialRequestDTO cartaNotarialRequestDTO);
        Task<CartaNotarial1ViewDTO> CartaNotarialSelect(int nIdent_CartaNotarial);
        Task<IEnumerable<ClientesListCbxDTO>> CartaNotarialPersonaList(int nIdent_CartaNotarial);
        /*
         Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa);
        Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana);
        Task<ContratosDTO> ObtenerContratoPorLote(int nIdent_Lote);
        Task<List<ClientesListCbxDTO>> ListarClientesPorContrato(int nIdent_Contrato);
        Task<int> CartaNotarialCreate(CartaNotarialDTO cartaNotarialDTO);
        Task<int> CartaNotarialUpdate(CartaNotarialDTO cartaNotarialDTO);
        Task<int> CartaNotarialDetalleCreate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO);
        Task<int> CartaNotarialDetalleUpdate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO);
        Task<int> CartaNotarialPersonaCreate(CartaNotarialPersonaDTO cartaNotarialPersonaDTO);
        Task<int> CartaNotarialPersonaDelete(int nIdent_CartaNotarial, int nIdent_UsuarioModificacion);
        Task<CartaNotarialDTO> CartaNotarialSelect(int nIdent_CartaNotarial);
        */
    }
}
