using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class KardexViewModel
    {
        public int Ident_Kardex { get; set; }
        public int Ident_Contratos { get; set; }
        public int Correlativo { get; set; }
        public decimal ImporteTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public int CantidadCuotas { get; set; }
        public int CuotaActual { get; set; }
        public decimal ImporteCuotas { get; set; }
        public decimal ImporteCuotaFinal { get; set; }
        public decimal TotalMoras { get; set; }
        public decimal MontoMorasPagado { get; set; }
        public decimal SaldoMorasPendientes { get; set; }

        public string Numero_Contrato { get; set; }


        public List<CuotasLista> CuotasListas { get; set; }
    }
}
