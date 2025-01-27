namespace InmobiliariaWeb.Result.Contratos
{
    public class Contrato_xNumeroSeparacion
    {
        public int Ident_Programa { get; set; }
        public string NombrePrograma { get; set; }
        public int  Ident_Manzana { get; set; }
        public string Manzana { get; set; }
        public int Ident_Lote { get; set; }
        public int Lote { get; set; }
        public decimal TratadoEn { get; set; }
        public decimal CuotaInicial { get; set; }
        public int CantidadCuotas { get; set; }
    }
}
