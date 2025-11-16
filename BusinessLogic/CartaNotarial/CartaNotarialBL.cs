using BusinessLogic.Interface.CartaNotarial;
using Domain.CartaNotarial;
using Domain.Contratos;
using Domain.Tablas;
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
        public async Task<IEnumerable<ItemCartaNotarialDetalleListDTO>> CartaNotarialDetalleList(int nIdent_CartaNotarial)
        { 
            return await _cartaNotarialRepository.CartaNotarialDetalleList(nIdent_CartaNotarial);
        }
        public async Task<int> CartaNotarialCreate(CartaNotarialDTO cartaNotarialDTO)
        { 
            return await _cartaNotarialRepository.CartaNotarialCreate(cartaNotarialDTO);
        }
        public async Task<int> CartaNotarialUpdate(CartaNotarialDTO cartaNotarialDTO)
        { 
            return await _cartaNotarialRepository.CartaNotarialUpdate(cartaNotarialDTO);
        }
        public async Task<int> CartaNotarialDetalleCreate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO)
        { 
            return await _cartaNotarialRepository.CartaNotarialDetalleCreate(cartaNotarialDetalleDTO);  
        }
        public async Task<int> CartaNotarialDetalleUpdate(CartaNotarialDetalleDTO cartaNotarialDetalleDTO)
        { 
            return await _cartaNotarialRepository.CartaNotarialDetalleUpdate(cartaNotarialDetalleDTO);
        }
        public async Task<int> CartaNotarialPersonaCreate(CartaNotarialPersonaDTO cartaNotarialPersonaDTO)
        { 
            return await _cartaNotarialRepository.CartaNotarialPersonaCreate(cartaNotarialPersonaDTO);
        }
        public async Task<int> CartaNotarialPersonaDelete(int nIdent_CartaNotarial, int nIdent_UsuarioModificacion)
        { 
            return await _cartaNotarialRepository.CartaNotarialPersonaDelete(nIdent_CartaNotarial, nIdent_UsuarioModificacion); 
        }
        public async Task<ContratoPorLoteDTO> ObtenerContratoPorLote(int nIdent_Lote)
        { 
            return await _cartaNotarialRepository.ObtenerContratoPorLote(nIdent_Lote);
        }
        public async Task<IEnumerable<ClientesListCbxDTO>> ListarClientesPorContrato(int nIdent_Contrato)
        { 
            return await _cartaNotarialRepository.ListarClientesPorContrato(nIdent_Contrato);
        }
    }
}
