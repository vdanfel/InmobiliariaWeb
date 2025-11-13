using Domain.Tablas;

namespace BusinessLogic.Interface.Adenda
{
    public interface IAdendaBL
    {
        Task<int> AdendasCreate(AdendasDTO adendasDTO);
        Task<AdendasDTO> ObtenerAdendaPendiente(AdendasDTO adendasDTO);
        Task<int> AdendasUpdate(AdendasDTO adendasDTO);
        Task<AdendasDTO> AdendasSelect(int nIdent_Adendas);
    }
}
