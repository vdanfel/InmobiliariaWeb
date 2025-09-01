using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Models.Egresos
{
    public class EgresosIndexViewModel
    {
        public DateTime? dFechaDesde { get; set; }
        public DateTime? dFechaHasta { get; set; }
        public string? sPersona { get; set; }
        public int? nIdent_022_TipoEgresos { get; set; }
        public List<TipoEgreso>lTipoEgreso { get; set; }
        public decimal? nTotalSoles { get; set; }
        public decimal? nTotalDolares { get; set; }
    }
}
