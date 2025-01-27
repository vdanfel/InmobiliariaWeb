using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Models.Contratos
{
    public class IndexViewModel
    {
        public int Correlativo { get; set; }
        public string NombrePrograma { get; set; }
        public int Ident_Programa { get; set; }
        public List<ProgramasCbxList> ProgramasCbxLists { get; set; }
        public string Manzana { get; set; }
        public int Ident_Manzana { get; set; }
        public string Cliente { get; set; }
        public int CantidadFilas { get; set; }
        public int PaginaActual { get; set; }
        public int GrupoActual { get; set; }
        public int NumeroPaginas { get; set; }
        public int NumeroGrupos { get; set; }
        public List<ContratosList>ContratosLists { get; set; }
    }
}
