namespace InmobiliariaWeb.Models.Ingresos
{
    public class IngresosIndexViewModel
    {
        public DateTime? dFechaDesde { get; set; }
        public DateTime? dFechaHasta { get; set; }
        public List<IngresosIndexTablaDTO> lIngresosTabla { get; set;}
    }
}
