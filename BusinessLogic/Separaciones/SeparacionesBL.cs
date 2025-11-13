using BusinessLogic.Interface.Separaciones;
using Repository.Interface.Separaciones;

namespace BusinessLogic.Separaciones
{
    public class SeparacionesBL: ISeparacionesBL
    {
        ISeparacionesRepository _separacionesRepository;
        public SeparacionesBL(ISeparacionesRepository separacionesRepository)
        {
            _separacionesRepository = separacionesRepository;
        }
    }
}
