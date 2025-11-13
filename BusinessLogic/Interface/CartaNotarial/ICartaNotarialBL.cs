using Domain.CartaNotarial;

namespace BusinessLogic.Interface.CartaNotarial
{
    public interface ICartaNotarialBL
    {
        Task<IEnumerable<CartaNotarialResponseDTO>> CartaNotarialBandeja(CartaNotarialRequestDTO cartaNotarialRequestDTO);
        Task<CartaNotarial1ViewDTO> CartaNotarialSelect(int nIdent_CartaNotarial);
        Task<IEnumerable<ClientesListCbxDTO>> CartaNotarialPersonaList(int nIdent_CartaNotarial);
    }
}
