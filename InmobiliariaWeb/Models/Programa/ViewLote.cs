using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Programa;

namespace InmobiliariaWeb.Models.Programa
{
    public class ViewLote
    {
        public int IdentPrograma { get; set; }
        public string NombrePrograma { get; set; }
        public string LetraManzana {  get; set; }
        public string Mensaje { get; set; }
        public List<LotesList> LotesList { get; set; }
        public List <TipoLote>TipoLote { get; set; }
    }
}
