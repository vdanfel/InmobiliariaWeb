namespace InmobiliariaWeb.Models.Egresos
{
    public class EgresosIndexFilterDTO
    {
        public DateTime? dFechaDesde { get; set; }
        public DateTime? dFechaHasta { get; set; }
        public string? sPersona { get; set; }
        public int? nIdent_022_TipoEgresos { get; set; }

    }
}
