namespace Domain.Tablas
{
    public class CartaNotarialPersonaDTO
    {
        public int? nIdent_CartaNotarialPersona { get; set; }
        public int? nIdent_CartaNotarial { get; set; }
        public int? nIdent_Persona { get; set; }
        public bool? bActivo { get; set; }
        public int? nIdent_UsuarioCreacion { get; set; }
        public DateTime? dFechaCreacion { get; set; }
        public int? nIdent_UsuarioModificacion { get; set; }
        public DateTime? dFechaModificacion { get; set; }
    }
}
