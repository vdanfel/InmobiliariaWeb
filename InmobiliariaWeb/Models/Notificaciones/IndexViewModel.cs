using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Notificaciones
{
    public class IndexViewModel
    {
        public string NombrePersona { get; set; }
        public int Ident_Programa { get; set; }
        public string Manzana { get; set; }
        public string Estado { get; set; }
        public List<ProgramasCbxList> ProgramasCbxLists { get; set; }
        public List<string> EstadosClienteCbxList { get; set; }
        public List<NotificacionesList>NotificacionesLists { get; set; }
    }
}
