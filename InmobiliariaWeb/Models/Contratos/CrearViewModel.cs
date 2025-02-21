using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Contratos
{
    public class CrearViewModel
    {
        public List<ProgramasCbxList> ProgramasCbxLists { get; set; }
        public int Ident_Lote { get; set; }
        public DateTime FechaContrato { get; set; }
        public string NumeroSeparacion { get; set; }
        public decimal TratadoEn { get; set; }
        public decimal CuotaInicial { get; set; }
        public decimal SaldoAPagar { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal CuotasIniciales { get; set; }
        public decimal CuotaFinal { get; set; }
        public DateTime FechaCuotaInicial { get; set; }
        public string Observacion { get; set; }
        public string Mensaje { get;set; }
    }
}
