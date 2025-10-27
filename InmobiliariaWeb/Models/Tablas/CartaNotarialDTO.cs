namespace InmobiliariaWeb.Models.Tablas
{
    public class CartaNotarialDTO
    {
        public int? nIdent_CartaNotarial { get; set; }
        public int? nIdent_Contratos { get; set; }
        public string? sSerie { get; set; }
        public int? nCorrelativo { get; set; }
        public DateTime? dFechaCartaNotarial { get; set; }
        public string? sNombreNotaria { get; set; }
        public string? sObservacion { get; set; }
        public int? nIdent_UsuarioCreacion { get; set; }
        public DateTime? dFechaCreacion { get; set; }
        public int? nIdent_UsuarioModificacion { get; set; }
        public DateTime? dFechaModificacion { get; set; }
    }
}
