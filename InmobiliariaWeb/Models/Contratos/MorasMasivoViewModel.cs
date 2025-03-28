﻿using InmobiliariaWeb.Models.Tablas;

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
    }
}
