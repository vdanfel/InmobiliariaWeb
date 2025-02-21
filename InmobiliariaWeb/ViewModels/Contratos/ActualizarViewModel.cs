using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Models.Programa;

namespace InmobiliariaWeb.ViewModels.Contratos
{
    public class ActualizarViewModel
    {
        public ContratosModel ContratosModels { get; set; }
        public string NumeroSeparacion { get; set; }
        public string Mensaje { get; set; }
        public string NumeroSerie { get; set; }
        public ProgramaModel ProgramaModels { get; set; }
        public string Manzana { get; set; }
        public int Lote { get;set; }
        public decimal Area { get; set; }
        public string Ubicacion { get; set; }
        public decimal PrecioM2 { get; set; }
        public decimal PrecioTotal { get; set; }
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
        public int ident_010_TipoLote { get; set; }
    }
}
