using InmobiliariaWeb.Result.Programa;

namespace InmobiliariaWeb.Models.Programa
{
    public class ViewManzana
    {
        public int IdentPrograma { get; set; }
        public string NombrePrograma { get; set; }
        public string Mensaje { get; set; }
        public List<ManzanaList> ManzanaList { get; set;}
    }
}
