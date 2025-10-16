namespace InmobiliariaWeb.Models.Kardex
{
    public class KardexNuevoDTO
    {
        public int? nIdent_Kardex { get; set; }
        public int? nIdent_Adendas { get; set; }
        public int? nIdent_Contratos { get; set; }
        public string? sNombrePrograma { get; set; }
        public string? sManzana { get; set; }
        public int? nLote { get; set; }
        public decimal? nSaldoPendiente { get; set; }
        public decimal? nSaldoMorasPendientes { get; set; }
        public decimal? nTotalDeuda { get; set; }
        public decimal? nIncremento { get; set; }
        public string? sIncremento { get; set; }
        public decimal? nDescuento { get; set; }
        public string? sDescuento { get; set; }
        public decimal? nImporteNuevo { get; set; }
        public int? nCuotas { get; set; }
        public decimal? nImporteMensual { get; set; }
        public decimal? nImporteFinal {  get; set; }
        public DateTime? dFechaInicio { get; set; }
        public int? nDiaPago { get; set; }
        public int? nUsuarioCreacion { get; set; }
    }
}
