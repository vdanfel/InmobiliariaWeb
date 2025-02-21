using InmobiliariaWeb.Models.Tablas;
using System.Threading.Tasks;

namespace InmobiliariaWeb.Interfaces
{
    public interface ITablasService
    {
        Task<List<TipoEstadoCivil>> ListarTipoEstadoCivil();
        Task<List<TipoDocumento>> ListarTipoDocumento();
        Task<List<Departamento>> ListarDepartamento();
        Task<List<Provincia>> ListarProvincia(string codigoDepartamento);
        Task<List<Distrito>> ListarDistrito(string codigoDepartamento, string codigoProvincia);
        Task<List<TipoPropietario>> ListarTipoPropietario();
        Task<List<Manzanas>> ListarManzanas();
        Task<List<TipoLote>> listarTipoLote();
        Task<List<TipoRol>> ListarTipoUsuarios();
        Task<List<TipoUbicacionLote>> ListarTipoUbicacionlote();
        Task<List<TipoSexo>> ListarSexo();
        Task<List<Paises>> ListarPaises();
        Task<List<TipoContrato>> ListarTipoContrato();
        Task<List<TipoLado>> ListarLado();
        Task<List<TipoPago>> ListarTipoPago();
        Task<List<Banco>> ListarBancos();
        Task<List<TipoCuentaBanco>> ListarTipoCuentaBanco();
        Task<List<TipoMoneda>> ListarTipoMoneda();
    }
}
