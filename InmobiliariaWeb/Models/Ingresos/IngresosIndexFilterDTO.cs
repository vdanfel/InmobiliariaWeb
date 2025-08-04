namespace InmobiliariaWeb.Models.Ingresos
{
    public class IngresosIndexFilterDTO
    {
        public DateTime? dFechaDesde { get; set; }
        public DateTime? dFechaHasta { get; set; }
        public int? nTipoIngreso { get; set; }
        public int? nIdent_Programa { get; set; }
        public int? nIdent_Manzana { get; set; }
        public int? nIdent_Lote { get; set; }
        public int? nIdent_Persona { get; set; }
    }
}
