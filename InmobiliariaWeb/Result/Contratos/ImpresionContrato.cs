namespace InmobiliariaWeb.Result.Contratos
{
    public class ImpresionContrato
    {
        public int IdentContrato { get; set; }
        public string DenominacionVendedores { get; set; }
        public string NombreFormatoImpresion { get; set; }
        public List<TablaPropietarios> Propietarios { get; set; }
        public List<TablaClientes> Clientes { get; set; }
        public List<TablaParrafos> Clausulas { get; set; }
    }

    public class TablaPropietarios
    {
        public string Honorifico_Vendedor { get; set; }
        public string Nombre_Vendedor { get; set; }
        public string Nacionalidad_Vendedor { get; set; }
        public string Identifica_Vendedor { get; set; }
        public string Tipo_Documento_Vendedor { get; set; }
        public string Documento_Vendedor { get; set; }
        public string Estado_Civil_Vendedor { get; set; }
        public string Direccion_Vendedor { get; set; }
        public string Distrito_Vendedor { get; set; }
        public string Provincia_Vendedor { get; set; }
        public string Departamento_Vendedor { get; set; }
        public string Denominacion_Vendedor { get; set; }
    }

    public class TablaClientes
    {
        public string Honorifico_Comprador { get; set; }
        public string Nombre_Comprador { get; set; }
        public string Denominacion_Comprador { get; set; }
    }

    public class TablaParrafos
    {
        public int Correlativo { get; set; }
        public string Detalle { get; set; }
    }
}
