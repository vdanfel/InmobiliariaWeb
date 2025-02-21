using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Separaciones
{
    public class CrearViewModel
    {
        public int Ident_Separaciones { get; set; }
        public List<ProgramasCbxList> ProgramasCbxLists {  get; set; }
        public int Ident_Lote { get;set; }
        public DateTime FechaSeparacion { get; set; }
        public decimal CuotaInicial{ get; set; }
        public decimal SaldoAPagar{ get; set; }
        public int CantidadCuotas { get; set; }
        public decimal CuotasIniciales { get; set; }
        public decimal CuotaFinal { get; set; }
        public string Observacion { get; set; }
        public decimal ImporteSeparacion { get; set; }
        public int DiasSeparacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal TratadoEn { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal ImporteSeparacionDolares { get; set; }
    }
}
