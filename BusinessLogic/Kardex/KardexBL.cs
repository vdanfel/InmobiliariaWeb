using BusinessLogic.Interface.Kardex;
using Domain.Adendas;
using Repository.Interface.Kardex;

namespace BusinessLogic.Kardex
{
    public class KardexBL: IKardexBL
    {
        IKardexRepository _kardexRepository;
        public KardexBL(IKardexRepository kardexRepository)
        {
            _kardexRepository = kardexRepository;
        }
        public async Task<int> KardexReprogramacion(ValoresReprogramacionDTO valoresReprogramacionDTO)
        { 
            return await _kardexRepository.KardexReprogramacion(valoresReprogramacionDTO);
        }
    }
}
