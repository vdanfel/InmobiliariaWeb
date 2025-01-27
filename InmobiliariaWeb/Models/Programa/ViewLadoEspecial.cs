using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Models.Programa
{
    public class ViewLadoEspecial
    {
        public int Ident_Lados { get; set; }
        public int Ident_Lote { get; set; }
        public int Ident_010_TipoLote { get; set; }
        public decimal L1 { get; set; }
        public int L1_Ident_009_Lado { get; set; }
        public string ColindaL1 {get; set; }
        public decimal L2 { get; set; }
        public int L2_Ident_009_Lado { get; set; }
        public string ColindaL2 { get; set; }
        public decimal L3 { get; set; }
        public int L3_Ident_009_Lado { get; set; }
        public string ColindaL3 { get; set; }
        public decimal L4 { get; set; }
        public int L4_Ident_009_Lado { get; set; }
        public string ColindaL4 { get; set; }
        public decimal L5 { get; set; }
        public int L5_Ident_009_Lado { get; set; }
        public string ColindaL5 { get; set; }
        public decimal L6 { get; set; }
        public int L6_Ident_009_Lado { get; set; }
        public string ColindaL6 { get; set; }
        public decimal L7 { get; set; }
        public int L7_Ident_009_Lado { get; set; }
        public string ColindaL7 { get; set; }
        public decimal L8 { get; set; }
        public int L8_Ident_009_Lado { get; set; }
        public string ColindaL8 { get; set; }
        public decimal L9 { get; set; }
        public int L9_Ident_009_Lado { get; set; }
        public string ColindaL9 { get; set; }
        public decimal L10 { get; set; }
        public int L10_Ident_009_Lado { get; set; }
        public string ColindaL10 { get; set; }
        public decimal Area { get; set; }
        public decimal PrecioM2 { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal Porcentaje { get; set; }
        public string Mensaje { get; set; }
        public int Ident_012_EstadoLote { get; set; }
        public string NombrePrograma { get; set; }
        public string LetraManzana { get; set; }
        public int NumeroLote { get; set; }
        public bool FlagCheked { get; set; }
        public int Ident_014_UbicacionLote { get; set; }
        public bool Flag_ReservadoPropietarpio { get; set; }
        public List<TipoUbicacionLote> TipoUbicacionLotes { get; set; }
        public List<TipoLado> tipoLados { get; set; }
    }
}
