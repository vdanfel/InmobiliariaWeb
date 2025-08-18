namespace InmobiliariaWeb.Models.Ingresos
{
    public class IngresosIndexTablaDTO
    {
        public int nIdent_Ingresos { get; set; }
        public string? sPrograma { get; set; }
        public string? sManzana { get; set; }
        public int? nLote { get; set; }
        public string? sNombreCliente { get; set; }
        public string? sTipoIngreso { get; set; }
        public string? sMoneda { get; set; }
        public decimal nImporte { get; set; }
        public string sTipoPago { get; set; }
        public string? sNumeroOperacion { get; set; }
        public DateTime dFechaPago { get; set; }
        public int? nIdent_002_TipoMoneda { get; set; }
    }
}
