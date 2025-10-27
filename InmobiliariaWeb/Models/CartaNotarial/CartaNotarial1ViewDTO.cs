using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.CartaNotarial
{
    public class CartaNotarial1ViewDTO
    {
        public int? nIdent_CartaNotarial {  get; set; }
        public int? nIdent_Contratos {  get; set; }
        public string? sNumeroCartaNotarial { get; set; }
        public int? nIdent_Programa { get; set; }
        public List<ProgramasCbxList> lPrograma { get; set; } = new List<ProgramasCbxList>();
        public int? nIdent_Manzana{ get; set; }
        public List<ManzanaCbxList>lManzana { get; set; } = new List<ManzanaCbxList>();
        public int? nIdent_Lote{ get; set; }
        public List<LoteCbxList>lLote { get; set; } = new List<LoteCbxList>();
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
