using InmobiliariaWeb.Models;
using InmobiliariaWeb.Result;

namespace InmobiliariaWeb.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResult> ValidarLogin(string Usuario, string Clave);
        Task AnularSeparacionesVencidas();
        Task ActualizarDiasMoras();
        Task ActualizarTotalesKardex();
        Task<bool> CargasIniciales();
    }
}
