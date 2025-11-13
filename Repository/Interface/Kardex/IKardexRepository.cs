using Domain.Adendas;

namespace Repository.Interface.Kardex
{
    public interface IKardexRepository
    {
        Task<int> KardexReprogramacion(ValoresReprogramacionDTO valoresReprogramacionDTO);
    }
}
