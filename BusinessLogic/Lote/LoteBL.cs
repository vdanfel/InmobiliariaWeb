using BusinessLogic.Interface.Lote;
using Domain.Lote;
using Repository.Interface.Lote;

namespace BusinessLogic.Lote
{
    public class LoteBL: ILoteBL
    {
        ILoteRepository _loteRepository;
        public LoteBL(ILoteRepository loteRepository)
        {
            _loteRepository = loteRepository;
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteLibreOpciones(int nIdent_Manzana)
        { 
            return await _loteRepository.LoteLibreOpciones(nIdent_Manzana);
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteConContratoOpciones(int nIdent_Manzana)
        { 
            return await _loteRepository.LoteConContratoOpciones(nIdent_Manzana);
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteConSeparacionOpciones(int nIdent_Manzana)
        { 
            return await _loteRepository.LoteConSeparacionOpciones(nIdent_Manzana);
        }
        public async Task<IEnumerable<LoteOpcionesDTO>> LoteConCartaNotarialOpciones(int nIdent_Manzana)
        { 
            return await _loteRepository.LoteConCartaNotarialOpciones(nIdent_Manzana);
        }
    }
}
