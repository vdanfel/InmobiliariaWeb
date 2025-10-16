namespace InmobiliariaWeb.Models.Tablas
{
    public class KardexDTO
    {
        public int? nIdent_Kardex { get; set; }
        public int? nIdent_Contratos { get; set; }
        public int? nIdent_Adendas { get; set; }
        public int? nCorrelativo { get; set; }
        public decimal? nImporteTotal { get; set; }
        public int? nCantidadCuotas { get; set; }
        public int? nCuotaActual { get; set; }
        public decimal? nMontoPagado { get; set; }
        public decimal? nSaldoPendiente { get; set; }
        public decimal? nTotalMoras { get; set; }
        public decimal? nMontoMorasPagado { get; set; }
        public decimal? nSaldoMorasPendientes { get; set; }
        public int? nIdent_004_Estado { get; set; }
        public int? nUsuarioCreacion { get; set; }
        public int? nUsuarioModificacion { get; set; }
    }
}
