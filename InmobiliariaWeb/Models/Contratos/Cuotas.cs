using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class Cuotas
    {
        public int Ident_Contratos { get; set; }
        public int Ident_Ingresos { get; set; }
        public int Ident_Cuotas { get; set; }
        public int Ident_Kardex { get; set; }
        public int Correlativo { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal ImporteCuota { get; set; }
        public DateTime? FechaPagoRealizado { get; set; }
        public decimal? ImporteCuotaPagado { get; set; }
        public string Observacion { get; set; }
        public int DiasMoras { get; set; }
        public int? Ident_015_EstadoPago { get; set; }
        public int Ident_004_Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public List<TipoPago>TipoPagos { get; set; }
        public int Ident_018_TipoPago { get; set; }
        public int Ident_019_Banco { get; set; }
        public string NumeroOperacion { get; set; }
        public List<Banco>Bancos { get; set; }
        public int Ident_020_TipoCuentaBanco { get; set; }
        public List<TipoCuentaBanco> TipoCuentaBancos { get; set; }
        public int Ident_CuentasBancarias { get; set; }
        public int Ident_002_TipoMoneda { get; set; }
        public List<TipoMoneda> TipoMonedas { get;set; }
        public decimal TipoCambio { get; set; }
        public decimal SubTotalSoles { get; set; }
        public decimal SubTotalDolares{ get; set; }
        public decimal ImporteTotalPagado { get; set; }
        public List<IngresosDetallesList> ingresosDetallesLists { get; set; }
        public decimal SaldoAPagar { get; set; }
        public decimal ImporteCuotasSoles { get; set; }
        public decimal ImporteCuotasDolares { get; set; }
    }
}
