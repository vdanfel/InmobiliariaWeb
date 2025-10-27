namespace InmobiliariaWeb.Models.Tablas
{
    public class ContratosDTO
    {
        public int? nIdent_Contratos { get; set; }
        public string? sSerie { get; set; }
        public int? nCorrelativo { get; set; }
        public int? nIdent_Separaciones { get; set; }
        public DateTime? dFechaContrato { get; set; }
        public int? nIdent_Lote { get; set; }
        public decimal? nTratadoEn { get; set; }
        public decimal? nCuotaInicial { get; set; }
        public decimal? nSaldoAPagar { get; set; }
        public int? nCantidadCuotas { get; set; }
        public decimal? nCuotasIniciales { get; set; }
        public decimal? nCuotaFinal { get; set; }
        public DateTime? dFechaCuotaInicial { get; set; }
        public int? nDiaCuota { get; set; }
        public string? sObservacion { get; set; }
        public string? sMotivoAnulacion { get; set; }
        public string? sDetalleAnulacion { get; set; }
        public bool? bEstadoImpresion { get; set; }
        public bool? bKardexCreado { get; set; }
        public bool? bFlag_Legalizado { get; set; }
        public int? nIdent_004_Estado { get; set; }
        public int? nUsuarioCreacion { get; set; }
        public int? nUsuarioModificacion { get; set; }
        /*-----*/
        public string? sNumeroContrato { get; set; }
        public string? nCuotasPendientes { get; set; }
    }
}
