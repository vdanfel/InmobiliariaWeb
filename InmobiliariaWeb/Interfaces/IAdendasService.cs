using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Interfaces
{
    public interface IAdendasService
    {
        Task<int> AdendasCreate(AdendasDTO adendasDTO);
        Task<AdendasDTO?> ObtenerAdendaPendiente(int nIdent_Contratos);
        Task<int> AdendasUpdate(AdendasDTO adendasDTO);
        Task<AdendasDTO> AdendasSelect(int nIdent_Adendas);
    }
}
