namespace InmobiliariaWeb.Models.Caja
{
    public class IngresosDetalleModel
    {
        public int Ident_IngresosDetalle { get; set; }
        public int Ident_Ingresos { get; set; }
        public int Ident_018_TipoPago { get; set; }
        public int Ident_CuentasBancarias { get; set; }
        public decimal Importe{ get; set; }
        public decimal ImporteConTC{ get; set; }
        public string NumeroOperacion { get; set; }
        public DateTime Fecha { get; set; }
        public int Ident_002_TipoMoneda { get; set; }
        public decimal TipoCambio { get; set; }
    }
}
