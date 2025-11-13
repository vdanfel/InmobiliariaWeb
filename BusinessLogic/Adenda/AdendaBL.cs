using BusinessLogic.Interface.Adenda;
using Domain.Tablas;
using Repository.Interface.Adenda;

namespace BusinessLogic.Adenda
{
    public class AdendaBL: IAdendaBL
    {
        IAdendaRepository _adendaRepository;
        public AdendaBL(IAdendaRepository adendaRepository)
        {
            _adendaRepository = adendaRepository;
        }
        public async Task<int> AdendasCreate(AdendasDTO adendasDTO)
        { 
            return await _adendaRepository.AdendasCreate(adendasDTO);
        }
        public async Task<AdendasDTO> ObtenerAdendaPendiente(AdendasDTO adendasDTO)
        { 
            return await _adendaRepository.ObtenerAdendaPendiente(adendasDTO);
        }
        public async Task<int> AdendasUpdate(AdendasDTO adendasDTO)
        {
            return await _adendaRepository.AdendasUpdate(adendasDTO);
        }
        public async Task<AdendasDTO> AdendasSelect(int nIdent_Adendas)
        {
            return await _adendaRepository.AdendasSelect(nIdent_Adendas);
        }
    }
}
