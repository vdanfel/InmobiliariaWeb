using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Usuario;

namespace InmobiliariaWeb.Models.Usuario
{
    public class UsuarioViewModel
    {
        public string Buscar { get; set; }
        public int IdentPersona { get; set; }
        public string Mensaje { get; set; }
        public int Ident_012_TipoEstadoCivil { get; set; }
        public List<TipoRol>TipoRol { get; set; }
        public List<UsuarioList> UsuarioList { get; set;}
    }
}
