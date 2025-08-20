namespace InmobiliariaWeb.Models.Caja
{
    public class IngresosModel
    {
        public int Ident_Ingresos { get; set; }
        public int Ident_021_TipoIngresos { get; set; }
        public int Ident_Origen { get; set; }
        public int Ident_Programa { get; set; }
        public int Ident_Manzana{ get; set; }
        public int Ident_Lote { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal ImporteTotal { get; set; }
        public int Ident_002_TipoMoneda { get; set; }
        public int Ident_015_EstadoPago { get; set; }
        public string Observacion { get; set; }
        public int Ident_Persona { get; set; }
    }
}
