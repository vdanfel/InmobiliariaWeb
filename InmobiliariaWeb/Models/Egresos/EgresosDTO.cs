namespace InmobiliariaWeb.Models.Egresos
{
    public class EgresosDTO
    {
        public int? nIdent_Egresos { get; set; }
        public int? nIdent_022_TipoEgresos { get; set; }
        public int? nIdent_Persona { get; set; }
        public string? sObservacion { get; set; }
        public DateTime? dFechaEgreso { get; set; }
        public int? nIdent_002_TipoMoneda { get; set; }
        public decimal? nImporteTotal { get; set; }
        public int? nIdent_004_Estado { get; set; }
        public int? nUsuarioCreacion { get; set; }
        public int? nUsuarioModificacion { get; set; }
    }
}
