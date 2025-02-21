namespace InmobiliariaWeb.Result.Contratos
{
    public class CuotasLista
    {
        public int Ident_Cuotas { get; set; }
        public int Correlativo { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal ImporteCuota { get; set; }
        public DateTime? FechaPagoRealizado { get; set; }
        public decimal ImporteCuotaPagado { get; set; }
        public int DiasMoras { get; set; }
        public decimal ImporteMoras { get; set; }
        public DateTime? FechaMorasPagadas { get; set; }
        public decimal ImporteMorasPagadas { get; set; }
        public int Ident_015_EstadoPago { get; set; }

    }
}
