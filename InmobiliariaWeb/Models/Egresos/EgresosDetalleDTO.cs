namespace InmobiliariaWeb.Models.Egresos
{
    public class EgresosDetalleDTO
    {
        public int? nIdent_EgresosDetalle { get; set; }
        public int? nIdent_Egresos { get; set; }
        public int? nIdent_018_TipoPago { get; set; }
        public int? nIdent_019_Banco { get; set; }
        public int? nIdent_002_TipoMoneda { get; set; }
        public DateTime? dFecha { get; set; }
        public decimal? nImporte { get; set; }
        public string? sNumeroOperacion { get; set; }
        public int? nIdent_004_Estado { get; set; }
        public int? nUsuarioCreacion { get; set; }
        public int? nUsuarioModificacion { get; set; }
    }
}
