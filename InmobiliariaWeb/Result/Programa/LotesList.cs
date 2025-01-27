namespace InmobiliariaWeb.Result.Programa
{
    public class LotesList
    {
        public int IdentLote { get; set; }
        public int Indice { get; set; }
        public int Correlativo{ get; set; }
        public int CantidadLados { get; set; }
        public int Ident010TipoLote { get; set; }
        public decimal PrecioM2 { get; set; }
        public decimal Area {  get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal Porcentaje { get; set; }
        public int Ident012EstadoLote { get; set; }
        public int Ident014UbicacionLote { get; set; }
        public string TipoUbicacion { get; set; }
        public bool Flag_ReservadoPropietarpio { get; set; }
    }
}
