namespace InmobiliariaWeb.Models.Egresos
{
    public class EgresosDetallesList
    {
        public int? nIdent_EgresosDetalle { get; set; }
        public string? sNombrePersona { get; set; }
        public string? sDescripcion { get; set; }
        public string? sMoneda { get; set; }
        public decimal? nImporte { get; set; }
        public string? sTipoPago { get; set; }
        public string? sNumeroOperacion { get; set; }
        public string? sBanco { get; set; }
    }
}
