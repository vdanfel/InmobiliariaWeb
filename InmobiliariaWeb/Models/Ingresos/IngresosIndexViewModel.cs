using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Ingresos
{
    public class IngresosIndexViewModel
    {
        public DateTime? dFechaDesde { get; set; }
        public DateTime? dFechaHasta { get; set; }
        public List<IngresosIndexTablaDTO> lIngresosTabla { get; set;}
        public List<ProgramasCbxList> ProgramasCbxLists { get; set; }
        public int Ident_Programa { get; set; }
        public int Ident_Manzana { get; set; }
        public int Ident_Lote { get; set; }
        public int Ident_Persona { get; set; }
        public List<TipoIngreso>lTipoIngreso { get; set; }
        public int nIdent_021_TipoIngresos { get; set; }
        public decimal nTotalSoles { get; set; }
        public decimal nTotalDolares { get; set; }
    }
}
