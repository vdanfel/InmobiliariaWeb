using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Ingresos
{
    public class IngresosViewModel
    {
        public int nIdent_Ingresos { get; set; }
        public List<ProgramasCbxList> lProgramasCbxLists { get; set; }
        public int nIdent_Programa { get; set; }
        public int nIdent_Manzana { get; set; }
        public int nIdent_Lote { get; set; }
        public int nIdent_Persona { get; set; }
        public List<TipoIngreso> lTipoIngreso { get; set; }
        public int nIdent_021_TipoIngresos { get; set; }
        public List<TipoPago> lTipoPagos { get; set; }
        public int nIdent_018_TipoPago { get; set; }
        public List<Banco> lBancos { get; set; }
        public int nIdent_019_Banco { get; set; }
        public List<TipoMoneda> lTipoMonedas { get; set; }
        public List<TipoMoneda> lTipoMonedasCabecera { get; set; }
        public int nIdent_002_TipoMonedaCabecera { get; set; }
        public int nIdent_002_TipoMoneda { get; set; }
        public DateTime dFechaIngreso { get; set; }
        public string sObservacion { get; set; }
        public int nIdent_CuentasBancarias { get; set; }
        public string sNumeroOperacion { get; set; }
        public decimal nTipoCambio { get; set; }
        public decimal nImporte { get; set; }
        public string sNombreCompleto { get; set; }
        public string sDocumento{ get; set; }
        public List<IngresosDetallesList> lIngresosDetallesList { get; set; }
    }
}
