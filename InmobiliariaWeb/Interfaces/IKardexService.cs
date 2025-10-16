using InmobiliariaWeb.Models.Kardex;
using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Interfaces
{
    public interface IKardexService
    {
        
        Task<KardexNuevoDTO> DatosAdenda(int nIdent_Contratos);
        Task<int> KardexNuevoInsert(KardexNuevoDTO kardexNuevoDTO);
    }
}
