namespace InmobiliariaWeb.Models.Tablas
{
    public class RutaArchivoDTO
    {
        public int? nIdent_RutaArchivo { get; set; }
        public int? nIdent_024_TablaOrigen { get; set; }
        public int? nIdent_TablaOrigen { get; set; }
        public string? sNombreArchivo { get; set; }
        public string? sNombreGuardado { get; set; }
        public string? sExtension { get; set; }
        public decimal? nTamanio { get; set; }
        public string? sRutaArchivo { get; set; }
        public bool? bActivo { get; set; }
        public int? UsuarioCreacion { get; set; }
        public int? UsuarioModificacion { get; set; }
    }
}
