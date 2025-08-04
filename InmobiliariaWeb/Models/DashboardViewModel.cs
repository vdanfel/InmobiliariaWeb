using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models
{
    public class DashboardViewModel
    {
        public string IdentUsuario { get; set; }
        public string Nombre { get; set; }
        public string Mensaje { get; set; }
        public int Ident_Programa { get; set; }
        public string EstadoCliente { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<ProgramasCbxList> ProgramasCbxLists { get; set; }
        public List<int> AniosCbxList { get; set; }
        public List<string> EstadosClienteCbxList { get; set; }
        
    }
}
