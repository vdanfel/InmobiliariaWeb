using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.ViewModels.Programa
{
    public class CrearViewModel
    {
        public ProgramaModel ProgramaModel { get; set; }
        public List<Manzanas> Manzanas { get; set; }
        public List<TipoContrato> TipoContratos { get; set; }
        public string Mensaje { get; set; }
    }
}
