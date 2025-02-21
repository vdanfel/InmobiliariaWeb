using InmobiliariaWeb.Models.Notificaciones;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Notificaciones;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Interfaces
{
    public interface INotificacionesService
    {
        Task<List<ProgramasCbxList>> ProgramaCbxListar();
        Task<List<NotificacionesList>> NotificacionesListar(IndexViewModel indexViewModel);
        Task ConfirmarNotificacion(int Ident_Cuotas,int Ident_Persona, int Usuario);
        Task<List<NotificacionesExportar>> ExportarExcel(string NombrePersona, int Ident_Programa, string Manzana, string Estado);
    }
}
