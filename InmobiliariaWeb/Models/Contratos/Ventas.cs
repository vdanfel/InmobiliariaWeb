using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class Ventas
    {
        public int Ident_FormatoVenta { get; set; }
        public int Ident_Contrato { get; set; }
        public string Titulo { get; set; }
        public string ParrafoInicial { get; set; }
        public string Clausula1 { get; set; }
        public string Clausula2 { get; set; }
        public string Clausula3 { get; set; }
        public string Clausula4 { get; set; }
        public string Clausula5 { get; set; }
        public string Clausula6 { get; set; }
        public string Clausula7 { get; set; }
        public string ClausulaAllanamiento { get; set; }
        public string ClausulaDesalojo { get; set; }
        public string Clausula8 { get; set; }
        public string Clausula9 { get; set; }
        public string Clausula10 { get; set; }
        public string Clausula11 { get; set; }
        public string Clausula12 { get; set; }
        public string Clausula13 { get; set; }
        public string Clausula14 { get; set; }
        public string Clausula15 { get; set; }
        public string Clausula16 { get; set; }
        public string Clausula17 { get; set; }
        public string ClausulaAdicional { get; set; }
        public string TextoFrente { get; set; }
        public string TextoDerecha { get; set; }
        public string TextoIzquierda { get; set; }
        public string TextoFondo { get; set; }
        public string FechaContrato { get; set; }
        public List<Involucrados> Involucrados { get; set; } = new List<Involucrados>();
    }
}
