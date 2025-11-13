using BusinessLogic.Interface.CartaNotarial;
using Domain.CartaNotarial;
using Repository.Interface.CartaNotarial;

namespace BusinessLogic.CartaNotarial
{
    public class CartaNotarialBL: ICartaNotarialBL
    {
        ICartaNotarialRepository _cartaNotarialRepository;
        public CartaNotarialBL(ICartaNotarialRepository cartaNotarialRepository)
        {
            _cartaNotarialRepository = cartaNotarialRepository;
        }
        public async Task<IEnumerable<CartaNotarialResponseDTO>> CartaNotarialBandeja(CartaNotarialRequestDTO cartaNotarialRequestDTO)
        { 
            return await _cartaNotarialRepository.CartaNotarialBandeja(cartaNotarialRequestDTO);
        }
        public async Task<CartaNotarial1ViewDTO> CartaNotarialSelect(int nIdent_CartaNotarial)
        {
            return await _cartaNotarialRepository.CartaNotarialSelect(nIdent_CartaNotarial);
        }
        public async Task<IEnumerable<ClientesListCbxDTO>> CartaNotarialPersonaList(int nIdent_CartaNotarial)
        {
            return await _cartaNotarialRepository.CartaNotarialPersonaList(nIdent_CartaNotarial);
        }
    }
}
