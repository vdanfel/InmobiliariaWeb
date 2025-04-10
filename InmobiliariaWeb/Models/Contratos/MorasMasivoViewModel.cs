using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class MorasMasivoViewModel
    {
        public int Ident_Kardex { get; set; }
        public decimal ImporteMorasTotal { get; set; }
        public decimal? DescuentoDirecto { get; set; }
        public decimal? DescuentoPorcentaje { get; set; }
        public decimal? NuevoMontoMora { get; set; }
        public decimal? TotalMoraPagado { get; set; }
        public decimal? ImporteAPagar { get; set; }
        public decimal? ImporteMorasDolares { get; set; }
        public decimal? ImporteMorasPagado { get; set; }
        public DateTime FechaPago { get; set; }
        public int? Ident_018_TipoPago { get; set; }
        public List<TipoPago> TipoPagos { get; set; }
        public int Ident_019_Banco { get; set; }
        public List<Banco> Bancos { get; set; }
        public int Ident_020_TipoCuentaBanco { get; set; }
        public List<TipoCuentaBanco> TipoCuentaBancos { get; set; }
        public int Ident_002_TipoMoneda { get; set; }
        public List<TipoMoneda> TipoMonedas { get; set; }
        public int Ident_CuentasBancarias { get; set; }
        public decimal TipoCambio { get; set; }
        public string NumeroOperacion { get; set; }
        public List<IngresosDetallesList> ingresosDetallesLists { get; set; }
        public decimal ImporteTotalPagado { get; set; }
        public decimal SaldoAPagar { get; set; }
        public string Observacion { get; set; }
    }
}
