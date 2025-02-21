using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.ViewModels.Programa;
using System.Threading.Tasks;

namespace InmobiliariaWeb.Interfaces
{
    public interface IProgramaService
    {
        Task<int> RegistrarPrograma(CrearViewModel crearViewModel, LoginResult loginResult);
        Task RegistrarManzanaInicial(int Ident_Programa, int usuario);
        Task<string> RegistrarManzanas(ProgramaModel programaModel, LoginResult loginResult);
        Task<ActualizarViewModel> BuscarProgramaIdentPrograma(int identPrograma);
        Task<List<ProgramaList>> BandejaPrograma(string buscar);
        Task<int> RegistrarPropietario(ViewPropietario viewPropietario, LoginResult loginResult);
        Task<List<PropietarioList>> ListarPropietario(int identPrograma);
        Task<List<ManzanaList>> ListarManzanasPrograma(int ident_Programa);
        Task<string> ValidarManzanaInicial(int Ident_Programa, int ManzanaInicial, int CantidadManzanas);
        Task<string> AnularPrograma(int Ident_Programa);
        Task<string> ActualizarPrograma(ProgramaModel programaModel, LoginResult loginResult);
        Task<string> AnularManzanasList(int IdentPrograma, int IdentUsuario);
        Task<string> ActualizarCantidadLotes(int IdentManzana, int CantidadLotes);
        Task<string> AnularPropietario(int IdentProgramPropietario, int identUsuario);
        Task<string> RegistrarLote(int Ident_Manzana, int CantidadLotes, LoginResult loginResult, int Ident_014_Ubicacionlote);
        Task<List<LotesList>> ListarLotes(int identManzana);
        Task<string> LadoRegularRegistrar(ViewLadoregular viewLadoregular, LoginResult loginResult);
        Task<string> LoteActualizar(int Ident_Lote, int Ident_010_TipoLote, decimal PrecioM2, decimal Area, decimal PrecioTotal, LoginResult loginResult, int Ident_012_EstadoLote, int Ident_014_Ubicacionlote, bool Flag_ReservadoPropietarpio);
        Task<ViewLadoregular> LadoRegularSelect(int IdentLote);
        Task<string> LadoEspecialRegistrar(ViewLadoEspecial viewLadoEspecial, LoginResult loginResult);
        Task<ViewLadoEspecial> LadoEspecialSelect(int IdentLote);
        Task<List<ReporteProgramasEstado>> ReporteProgramasxEstado(int Ident_Programa,int TipoReporte);
        Task<ProManLotList> DatosProManLot(int Ident_Programa, int Ident_Manzana, int Ident_Lote);
    }
}
