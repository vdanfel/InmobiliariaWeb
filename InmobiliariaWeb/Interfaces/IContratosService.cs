using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Interfaces
{
    public interface IContratosService
    {
        Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa);
        Task<Lote_xIdentLote> LoteDetalle(int Ident_Lote);
        Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana);
        Task<List<ProgramasCbxList>> ProgramaCbxListar();
        Task<List<ContratosList>> BandejaContratos(IndexViewModel indexViewModel);
        Task<int> CrearContrato(CrearViewModel crearViewModel, LoginResult loginResult);
        Task ActualizarContrato(int Ident_Contrato, ActualizarViewModel actualizarViewModel, LoginResult loginResult);
        Task<Contrato_xNumeroSeparacion> ObtenerxSeparacion(int numeroSeparacion);
        Task InsertarClientesxSeparacion(string correlativo, int ident_Contrato, LoginResult loginResult);
        Task<int> CrearKardex(int ident_Contratos, LoginResult loginResult);
        Task CrearCuotas(int ident_Kardex, int correlativo, DateTime fechaPago, decimal ImporteCuota, LoginResult loginResult);
        Task<List<CuotasLista>> ListarCuotas(int ident_Kardex);
        Task<ActualizarViewModel> ContratoxIdentContrato(int ident_Contrato);
        Task<int> IdentKardexXIdentContrato(int Ident_Contrato);
        Task<List<ClienteList>> ClientesxContrato(ClienteViewModel clienteViewModel);
        Task<string> ClienteInsertar(ClienteViewModel clienteViewModel, LoginResult loginResult);
        Task<string> ClienteEliminar(int Ident_ContratosCliente, LoginResult loginResult);
        Task EstadoImpresion(int Ident_Contratos);
        Task<ImpresionContrato> ImprimirContrato(int Ident_Contratos);
        Task<Ventas> FormatoContratoVentas(int Ident_Contratos);
    }
}
