using InmobiliariaWeb.Models.Roles;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Roles;

namespace InmobiliariaWeb.Interfaces
{
    public interface IRolesService
    {
        Task<List<RolesList>> ListarRoles(int Ident_005_TipoUsuario);
        Task<RolesIndexViewModel> CrearRol(RolesIndexViewModel rolesIndexViewModel);
        Task CrearAccesos(RolesIndexViewModel rolesIndexViewModel, LoginResult loginResult);
        Task<List<PaginasList>> ListarAccesos(int ident_005_TipoUsuario);
    }
}
