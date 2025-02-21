using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Separaciones
{
    public class ClienteViewModel
    {
        public int Ident_Separaciones { get; set; }
        public string Numero_Separacion { get; set; }
        public string Mensaje { get; set; }
        public int Ident_Persona { get; set; }
        public List<ClientesList> Clientes { get; set;}
    }
}
