using BusinessLogic.Interface.Cuota;
using Repository.Interface.Cuota;

namespace BusinessLogic.Cuota
{
    public class CuotaBL: ICuotaBL
    {
        ICuotaRepository _cuotaRepository;

        public CuotaBL(ICuotaRepository cuotaRepository)
        {
            _cuotaRepository = cuotaRepository;
        }
        public async Task<int> CuotasCompletas(int nIdent_Kardex)
        { 
            return await _cuotaRepository.CuotasCompletas(nIdent_Kardex);
        }
    }
}
