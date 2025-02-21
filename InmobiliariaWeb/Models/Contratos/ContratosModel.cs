using MathNet.Numerics;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaWeb.Models.Contratos
{
    public class ContratosModel
    {
        public int Ident_Contratos { get; set; }
        public string Serie { get; set; }
        public int Correlativo { get; set; }
        public int Ident_Separaciones { get; set; }
        public DateTime FechaContrato { get; set; }
        public int Ident_Lote { get; set; }
        public decimal TratadoEn { get; set; }
        public decimal CuotaInicial { get; set; }
        public decimal SaldoAPagar { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal CuotasIniciales { get; set; }
        public decimal CuotaFinal { get; set; }
        public DateTime FechaCuotaInicial { get; set; }
        public int DiaCuota { get; set; }
        public string Observacion { get; set; }
        public int MotivoAnulacion { get; set; }
        public string DetalleAnulacion { get; set; }
        public bool EstadoImpresion { get; set; }
        public bool KardexCreado { get; set; }
        public bool Flag_Legalizado { get; set; }
        public int Ident_004_Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Numero_Operacion { get; set; }
    }
}
