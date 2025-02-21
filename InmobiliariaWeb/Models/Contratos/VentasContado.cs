using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class VentasContado
    {
        public int Ident_Contrato { get; set; }
        public string Titulo { get; set; }
        public string ParrafoInicial { get; set; }
        public string Clausula1 { get; set; }
        public string Clausula2 { get; set; }
        public string Clausula3I { get; set; }
        public string TextoFrente { get; set; }
        public string TextoDerecha { get; set; }
        public string TextoIzquierda { get; set; }
        public string TextoFondo { get; set; }
        public string Clausula3F { get; set; }
        public string Clausula4 { get; set; }
        public string Clausula5 { get; set; }
        public string Clausula6 { get; set; }
        public string Clausula7 { get; set; }
        public string Clausula8 { get; set; }
        public string Clausula9 { get; set; }
        public string Clausula10 { get; set; }
        public string Clausula11 { get; set; }
        public string FechaContrato { get; set; }
        public List<Involucrados> Involucrados { get; set; } = new List<Involucrados>();
    }
}
