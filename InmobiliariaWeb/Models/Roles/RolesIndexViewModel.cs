using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Roles;

namespace InmobiliariaWeb.Models.Roles
{
    public class RolesIndexViewModel
    {
        public string NombreRol { get; set; }
        public int Ident_005_TipoUsuario { get; set; }
        public List <RolesList> RolesList { get; set; }
        public string Mensaje { get; set; }
    }
}
