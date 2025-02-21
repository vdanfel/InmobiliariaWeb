using InmobiliariaWeb.Result.Roles;

namespace InmobiliariaWeb.Models.Roles
{
    public class ViewGestionar
    {
        public string Mensaje { get;set; }
        public string NombreRol { get;set; }
        public int Ident_005_tipoUsuario { get;set; }
        public List<RolesList> RolesList{ get; set; }
        public List<PaginasList>PaginasLists { get;set; }
    }
}
