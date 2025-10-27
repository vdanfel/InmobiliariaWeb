namespace InmobiliariaWeb.Models.Tablas
{
    public class CartaNotarialDetalleDTO
    {
        public int? nIdent_CartaNotarialDetalle { get; set; }
        public int? nIdent_CartaNotarial { get; set; }
        public int? nIdent_026_EstadoCartaNotarial { get; set; }
        public string? sObservacion { get; set; }
        public bool? bActivo { get; set; }
        public int? nIdent_UsuarioCreacion { get; set; }
        public DateTime? dFechaCreacion { get; set; }
        public int? nIdent_UsuarioModificacion { get; set; }
        public DateTime? dFechaModificacion { get; set; }
    }
}
