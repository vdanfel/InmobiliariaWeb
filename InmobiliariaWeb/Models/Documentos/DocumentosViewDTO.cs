using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Models.Documentos
{
    public class DocumentosViewDTO
    {
        public IFormFile? fArchivoSubir { get; set; }
        public string? sNumero_Contrato { get; set; }
        public List<FormatosResponseDTO> lFormatos { get; set; } = new List<FormatosResponseDTO>();
        public List<RutaArchivoDTO> lRutaArchivo { get; set; } = new List<RutaArchivoDTO>();
    }
}
