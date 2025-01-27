using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class KardexViewModel
    {
        public int Ident_Kardex { get; set; }
        public int Ident_Contratos { get; set; }
        public int Correlativo { get; set; }
        public List<CuotasLista> CuotasListas { get; set; }
    }
}
