namespace InmobiliariaWeb.Models.Egresos
{
    public class EgresosIndexTablaDTO
    {
        public int nIdent_Egresos { get; set; }
        public int nIdent_EgresosDetalle { get; set; }
        public string sTipoEgresos { get; set; }
        public string sNombreCompleto { get; set; }
        public string sObservacion { get; set; }
        public string sMoneda { get; set; }
        public decimal nImporte { get; set; }
        public string sTipoPago { get; set; }
        public string sNumeroOperacion { get; set; }
        public DateTime dFechaEgreso { get; set; }
        public int nIdent_002_TipoMoneda { get; set; }
    }
}
