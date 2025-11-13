using BusinessLogic.Interface.Manzana;
using Domain.Manzana;
using Repository.Interface.Manzana;

namespace BusinessLogic.Manzana
{
    public class ManzanaBL: IManzanaBL
    {
        IManzanaRepository _manzanaRepository;

        public ManzanaBL(IManzanaRepository manzanaRepository)
        {
            _manzanaRepository = manzanaRepository;
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConContratoOpciones(int nIdent_Programa)
        { 
            return await _manzanaRepository.ManzanaConContratoOpciones(nIdent_Programa);
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConSeparacionesOpciones(int nIdent_Programa)
        { 
            return await _manzanaRepository.ManzanaConSeparacionesOpciones(nIdent_Programa);
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaLibreOpciones(int nIdent_Programa)
        { 
            return await _manzanaRepository.ManzanaLibreOpciones(nIdent_Programa);
        }
        public async Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConCartaNotarialOpciones(int nIdent_Programa)
        {
            return await _manzanaRepository.ManzanaConCartaNotarialOpciones(nIdent_Programa);
        }
    }
}
