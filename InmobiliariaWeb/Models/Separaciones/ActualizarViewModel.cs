using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Separaciones
{
    public class ActualizarViewModel
    {
        public int Ident_Separacion { get; set; }
        public DateTime FechaSeparacion { get; set; }
        public string Numero_Separacion { get;set; }
        public string Nombre_Programa { get; set; }
        public string Manzana { get; set; }
        public int Lote { get; set; }
        public decimal Area { get; set; }
        public string Ubicacion { get; set; }
        public decimal PrecioM2 { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal Izquierda { get; set; }
        public decimal Derecha { get; set; }
        public decimal Frente { get; set; }
        public decimal Fondo { get; set; }
        public decimal Lado1 { get; set; }
        public decimal Lado2 { get; set;}
        public decimal Lado3 { get; set; }
        public decimal Lado4 { get; set;}
        public decimal Lado5 { get; set;}
        public decimal Lado6 { get; set;}
        public decimal Lado7 { get; set;}
        public decimal Lado8 { get; set;}
        public decimal Lado9 { get; set;}
        public decimal Lado10 { get; set;}
        public decimal TratadoEn { get; set; }
        public decimal CuotaInicial { get; set; }
        public decimal SaldoAPagar { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal CuotasIniciales { get; set; }
        public decimal CuotaFinal { get; set; }
        public int DiasSeparacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal ImporteSeparacion { get; set; }
        public decimal TipoCambio { get; set; }
        public string Observacion { get; set; }
        public string MotivoActualizacion { get; set; }
        public string MotivoAnulacion { get; set; }
        public string Mensaje { get; set; }
        public int ident_010_TipoLote { get; set; }
    }
}
