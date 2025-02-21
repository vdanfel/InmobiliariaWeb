using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.ViewModels.Contratos
{
    public class CrearViewModel
    {
        public ContratosModel ContratosModels { get; set; }
        public string NumeroSeparacion { get; set; }
        public List<ProgramasCbxList> ProgramasCbxLists { get; set; }
        public string Mensaje { get; set; }
    }
}
