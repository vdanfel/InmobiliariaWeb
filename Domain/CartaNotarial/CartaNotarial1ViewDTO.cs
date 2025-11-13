using Domain.Lote;
using Domain.Manzana;
using Domain.Programa;

namespace Domain.CartaNotarial
{
    public class CartaNotarial1ViewDTO
    {
        public int? nIdent_CartaNotarial {  get; set; }
        public int? nIdent_Contratos {  get; set; }
        public string? sNumeroCartaNotarial { get; set; }
        public int? nIdent_Programa { get; set; }
        public int? nIdent_027_TipoCartaNotarial { get; set; }
        public int? nIdent_026_EstadoCartaNotarial { get; set; }
        public string? sSerie { get; set; }
        public int? nCorrelativo { get; set; }
        public List<ProgramaOpcionesDTO> lPrograma { get; set; } = new List<ProgramaOpcionesDTO>();
        public string? sNombrePrograma { get; set; }
        public string? sManzana { get; set; }
        public string? sLote { get; set; }
        public int? nIdent_Manzana{ get; set; }
        public List<ManzanaOpcionesDTO>lManzana { get; set; } = new List<ManzanaOpcionesDTO>();
        public int? nIdent_Lote{ get; set; }
        public List<LoteOpcionesDTO>lLote { get; set; } = new List<LoteOpcionesDTO>();
        public string? sNumeroContrato{ get; set; }
        public List<ClientesListCbxDTO> lClientes { get; set; } = new List<ClientesListCbxDTO>();
        public DateTime? dFechaCartaNotarial { get; set; }
        public string? sNombreNotaria { get; set; }
        public DateTime? dFechaEntregaNotaria { get; set; }
        public DateTime? dFechaSalidaNotaria { get; set; }
        public DateTime? dFechaRecojo { get; set; }
        public string? sObservacion { get; set; }
        public int nIdent_UsuarioCreacion { get; set; }
        public int nIdent_UsuarioModificacion { get; set; }
        public decimal nCuotasPendientes { get; set; }
    }
}
