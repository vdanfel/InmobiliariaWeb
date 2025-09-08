namespace InmobiliariaWeb.Utilities
{
    public class ReciboBE
    {
        public int nIdent_021_TipoIngresos { get; set; }
        public string NumeroRecibo { get; set; }
        public DateTime FechaPago { get; set; }
        public string NombreCompleto { get; set; }
        public string Documento { get; set; }
        public string? Observacion { get; set; }
        public decimal ImporteTotal { get; set; }
        public string NombreUsuario { get; set; }
        public string? NombrePrograma { get; set; }
        public string? Manzana { get; set; }
        public string? Lote { get; set; }
        public decimal TipoCambio { get; set; }
        public string Moneda { get; set; }
        public string? Correlativo { get; set; }
    }
}
