using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Usuario;

namespace InmobiliariaWeb.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioList>> ListarUsuario(string buscar);
        Task<UsuarioList> ListarUsuario_xIdentUsuario(int IdentUsuario);
        Task<int> RegistrarUsuario(UsuarioList usuarioList, LoginResult loginResult);
        Task<string> ActualizarUsuario(UsuarioList usuarioList, LoginResult loginResult, int tipoActualizar);
        Task<string> AnularUsuario(UsuarioList usuarioList, LoginResult loginResult);
    }
}
