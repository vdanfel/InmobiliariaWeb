using Domain.Tablas;

namespace Repository.Interface.Adenda
{
    public interface IAdendaRepository
    {
        Task<int> AdendasCreate(AdendasDTO adendasDTO);
        Task<AdendasDTO> ObtenerAdendaPendiente(AdendasDTO adendasDTO);
        Task<int> AdendasUpdate(AdendasDTO adendasDTO);
        Task<AdendasDTO> AdendasSelect(int nIdent_Adendas);
    }
}
