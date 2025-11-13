namespace Domain.CartaNotarial
{
    public class CartaNotarialResponseDTO
    {
        public int? nIdent_CartaNotarial { get; set; }
        public string? sNumeroCartaNotarial { get; set; }
        public string? sNombrePrograma { get; set; }
        public string? sManzana { get; set; }
        public string? sLote { get; set; }
        public string? sNumeroContrato { get; set; }
        public string? sNombreCliente { get; set; }
        public DateTime? dFechaCartaNotarial { get; set; }
    }
}
