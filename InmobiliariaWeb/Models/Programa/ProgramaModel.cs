namespace InmobiliariaWeb.Models.Programa
{
    public class ProgramaModel
    {
        public int Ident_Programa { get; set; }
        public string Serie { get; set; }
        public int Correlativo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Numero_Partida { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public decimal AreaTotal { get; set; }
        public decimal AreaLotizada { get; set; }
        public int CantidadManzanas { get; set; }
        public string Suministro { get; set; }
        public string NombreFormatoImpresion { get; set; }
        public int Ident_017_TipoContrato { get; set; }
        public int Ident_004_Estado { get; set; }
        public string Clausula1 { get; set; } = "";
        public decimal ? PorcentajeLiquidacion { get; set; }
        public int ManzanaInicial { get; set; }
    }
}
