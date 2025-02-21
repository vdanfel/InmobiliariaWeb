using InmobiliariaWeb.Models.Separaciones;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Interfaces
{
    public interface ISeparacionesService
    {
        Task<List<SeparacionesList>> BandejaSeparaciones(IndexViewModel indexViewModel);
        Task<List<ProgramasCbxList>> ProgramaCbxListar();
        Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa);
        Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana);
        Task<Lote_xIdentLote> LoteDetalle(int Ident_Lote);
        Task<int> SeparacionesInsert(CrearViewModel crearViewModel, LoginResult loginResult);
        Task<ActualizarViewModel> SeparacionXIdentSeparacion(int ident_Separacion);
        Task<string> ActualizarSeparacion(ActualizarViewModel actualizarViewModel, LoginResult loginResult);
        Task<List<ClientesList>> ClientexSeparacion(ClienteViewModel clienteViewModel);
        Task<string> ClienteInsertar(ClienteViewModel clienteViewModel, LoginResult loginResult);
        Task<string> ClienteEliminar(int Ident_SeparacionesCliente, LoginResult loginResult);
        Task<string> SeparacionesAnular(ActualizarViewModel actualizarViewModel, LoginResult loginResult);
        //Task AnularSeparacionesVencidas();
        Task<SeparacionesImpresionViewModel> ImprimirSeparaciones(int ident_Separacion);
        
    }
}
