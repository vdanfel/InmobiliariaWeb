namespace InmobiliariaWeb.Models.Contratos
{
    public class CuotasModel
    {
        public int Ident_Cuotas { get; set; }
        public int Ident_Kardex { get; set; }
        public int Correlativo { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal ImporteCuota { get; set; }
        public DateTime FechaPagoRealizado { get; set; }
        public decimal ImporteCuotaPagado { get; set; }
        public int Ident_018_TipoPago { get; set; }
        public string NumeroOperacion { get; set; }
        public string Observacion { get; set; }
        public int Ident_CuentasBancarias { get; set; }
        public int DiasMoras { get; set; }
        public int Ident_015_EstadoPago { get; set; }
        public int Ident_004_Estado { get; set; }
 
    }
}
