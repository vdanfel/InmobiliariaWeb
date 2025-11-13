namespace Domain.Adendas
{
    public class ValoresReprogramacionDTO
    {
        public int nIdent_Kardex { get; set; }
        public DateTime dNuevaFechaInicio { get; set; }
        public int nNuevoDiaPago { get; set; }
        public int nUsuarioModificacion { get; set; }
        public int nCuotaActual { get; set; }

    }
}
