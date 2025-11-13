using Domain.Programa;

namespace Domain.CartaNotarial
{
    public class CartaNotarialViewDTO
    {
        public List<CartaNotarialResponseDTO> lCartaNotarialList { get; set; }
        public List<ProgramaOpcionesDTO> lProgramas { get; set; }
        public int? nIdent_Programa { get; set; }
        public int? nIdent_Manzana { get; set; }
        public int? nIdent_Lote { get; set; }
        public string? sBuscar { get; set; }
    }
}
