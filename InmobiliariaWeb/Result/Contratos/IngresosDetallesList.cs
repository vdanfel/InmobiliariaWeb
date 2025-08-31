namespace InmobiliariaWeb.Result.Contratos
{
    public class IngresosDetallesList
    {
        public int Ident_IngresosDetalle { get; set; }
        public int Indice { get; set; }
        public string TipoPago { get; set; }
        public decimal ImporteConTC { get; set; }
        public decimal Importe { get; set; }
        public string NumeroOperacion { get; set; }
        public string Banco { get; set; }
        public string NumeroCuenta { get; set; }
        public string Moneda { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal TipoCambio { get; set; }
    }
}
