using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class ClienteViewModel
    {
        public int Ident_Contratos { get; set; }
        public string Numero_Contrato { get; set; }
        public string Mensaje { get; set; }
        public int Ident_Persona { get; set; }
        public bool EstadoImpresion { get; set; }
        public List<ClienteList> Clientes { get; set; }
    }
}
