using InmobiliariaWeb.Result.Dashboard;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Interfaces
{
    public interface IDashboardService
    {
        Task<List<ProgramasCbxList>> ProgramaCbxListar();
        Task<List<TotalesProgramas>> TTotalesProgramas(int Ident_Programa, int Anio, int Mes);
        Task<VSPeriodos> TVSPeriodos(int Ident_Programa, int Anio, int Mes);
    }
}
