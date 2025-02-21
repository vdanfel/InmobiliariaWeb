using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Models.Programa
{
    public class ViewLadoregular
    {
        public int Ident_Lados { get; set; }
        public int Ident_Lote { get; set; }
        public int Ident_010_TipoLote { get; set; }
        public int Ident_009_Lados { get; set; }
        public decimal Izquierda { get; set; }
        public string ColindaIzquierda { get;set; }
        public decimal Derecha { get; set; }
        public string ColindaDerecha { get; set; }
        public decimal Frente { get; set; }
        public string ColindaFrente { get; set; }
        public decimal Fondo { get; set; }
        public string ColindaFondo { get; set; }
        public decimal Area { get; set; }
        public decimal PrecioM2 { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal Porcentaje { get; set; }
        public string Mensaje { get; set; }
        public int Ident_012_EstadoLote { get; set; }
        public bool FlagCheked { get; set; }
        public string NombrePrograma { get; set; }
        public string LetraManzana { get; set; }
        public int NumeroLote { get; set; }
        public int Ident_014_UbicacionLote { get; set; }
        public bool Flag_ReservadoPropietarpio { get; set; }
        public List<TipoUbicacionLote> TipoUbicacionLotes { get; set; }
    }
}
