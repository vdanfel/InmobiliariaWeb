using Domain.Adendas;

namespace BusinessLogic.Interface.Kardex
{
    public interface IKardexBL
    {
        Task<int> KardexReprogramacion(ValoresReprogramacionDTO valoresReprogramacionDTO);
    }
}
