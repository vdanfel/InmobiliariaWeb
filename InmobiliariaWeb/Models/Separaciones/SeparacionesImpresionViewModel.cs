namespace InmobiliariaWeb.Models.Separaciones
{
    public class SeparacionesImpresionViewModel
    {
        public string NumeroSerie { get; set; }
        public DateTime FechaSeparacion { get; set; }
        public string NombrePrograma { get; set; }
        public string Manzana { get; set; }
        public int Lote { get; set; }
        public string Ubicacion { get; set; }
        public int TipoLote { get; set; }
        public decimal Izquierda { get; set; }
        public decimal Derecha { get; set; }
        public decimal Frente { get; set; }
        public decimal Fondo { get; set; }
        public decimal L1 { get; set; }
        public decimal L2 { get; set; }
        public decimal L3 { get; set; }
        public decimal L4 { get; set; }
        public decimal L5 { get; set; }
        public decimal L6 { get; set; }
        public decimal L7 { get; set; }
        public decimal L8 { get; set; }
        public decimal L9 { get; set; }
        public decimal L10 { get; set; }
        public decimal Area { get; set; }
        public decimal PrecioM2 { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal TratadoEn { get; set; }
        public decimal CuotaInicial { get; set; }
        public decimal SaldoAPagar { get; set; }
        public decimal CantidadCuotas { get; set; }
        public decimal CuotasIniciales { get; set; }
        public decimal CuotaFinal { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Observacion { get; set; }
        public decimal ImporteSeparacion { get; set; }
        public List<SeparacionesClientesImpresionViewModel> Clientes { get; set; }
    }
}
