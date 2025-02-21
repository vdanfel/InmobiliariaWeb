
using InmobiliariaWeb.Result;

namespace InmobiliariaWeb.Controllers
{
    public static class DatosLogin
    {
        public static LoginResult DatosUsuarioLogin(HttpContext httpContext)
        {
            LoginResult loginResult = null;

            if (httpContext.Session.GetInt32("IdentUsuario") != null)
            {
                loginResult = new LoginResult
                {
                    IdentUsuario = (int)httpContext.Session.GetInt32("IdentUsuario"),
                    Usuario = httpContext.Session.GetString("Usuario"),
                    Ident005TipoUsuario = (int)httpContext.Session.GetInt32("Ident005TipoUsuario"),
                    NombreCompleto = httpContext.Session.GetString("NombreCompleto")
                };
            }
            return loginResult;
        }
    }

}
