using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Models.Egresos
{
    public class EgresosViewModel
    {
        public int nIdent_Egresos { get; set; }
        public List<TipoEgreso> lTipoEgreso { get; set; }
        public int? nIdent_022_TipoEgresos { get; set; }
        public DateTime dFechaEgreso { get; set; }
        public string sObservacion { get; set; }
        public List<TipoPago> lTipoPagos { get; set; }
        public int nIdent_018_TipoPago { get; set; }
        public List<TipoMoneda> lTipoMonedas { get; set; }
        public int nIdent_002_TipoMoneda { get; set; }
        public decimal nImporte{ get; set; }
        public List<Banco> lBancos { get; set; }
        public int nIdent_019_Banco { get; set; }
        public string sNumeroOperacion { get; set; }
        public int? nIdent_Persona { get; set; }
        public string sNombrePersona { get; set; }
        public string sDocumento { get; set; }
        public List<EgresosDetallesList>lEgresosDetallesList { get; set; }
    }
}
