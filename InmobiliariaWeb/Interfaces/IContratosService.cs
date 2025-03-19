using InmobiliariaWeb.Models.Caja;
using InmobiliariaWeb.Models.Contratos;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;
using InmobiliariaWeb.ViewModels.Contratos;

namespace InmobiliariaWeb.Interfaces
{
    public interface IContratosService
    {
        Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa);
        Task<Lote_xIdentLote> LoteDetalle(int Ident_Lote);
        Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana);
        Task<List<ProgramasCbxList>> ProgramaCbxListar();
        Task<List<ContratosList>> BandejaContratos(IndexViewModel indexViewModel);
        Task<int> CrearContrato(ViewModels.Contratos.CrearViewModel crearViewModel, LoginResult loginResult);
        Task ActualizarContrato(int Ident_Contrato, ViewModels.Contratos.ActualizarViewModel actualizarViewModel, LoginResult loginResult);
        Task<Contrato_xNumeroSeparacion> ObtenerxSeparacion(int numeroSeparacion);
        Task InsertarClientesxSeparacion(string correlativo, int ident_Contrato, LoginResult loginResult);
        Task<int> CrearKardex(ViewModels.Contratos.ActualizarViewModel actualizarViewModel, LoginResult loginResult);
        Task CrearCuotas(int ident_Kardex, int correlativo, DateTime fechaPago, decimal ImporteCuota, LoginResult loginResult);
        Task<List<CuotasLista>> ListarCuotas(int ident_Kardex, LoginResult loginResult);
        Task<ViewModels.Contratos.ActualizarViewModel> ContratoxIdentContrato(int ident_Contrato);
        Task<int> IdentKardexXIdentContrato(int Ident_Contrato);
        Task<List<ClienteList>> ClientesxContrato(int Ident_Contratos);
        Task<string> ClienteInsertar(ClienteViewModel clienteViewModel, LoginResult loginResult);
        Task<string> ClienteEliminar(int Ident_ContratosCliente, LoginResult loginResult);
        Task EstadoImpresion(int Ident_Contratos);
        Task<ImpresionContrato> ImprimirContrato(int Ident_Contratos);
        Task FormatoVentas_Insert(int Ident_Contratos, LoginResult loginResult);
        Task<VentasContado> FormatoContratoVentasContado(int Ident_Contratos);
        Task<KardexViewModel> DatosKardex(int Ident_Kardex);
        Task CrearCuotasMasivas(int ident_Kardex, List<Cuotas> cuotas, int usuarioCreacion);
        Task<Cuotas> CuotasxIdentCuotas(int Ident_Cuotas);
        Task<string> CuotasActualizar(Cuotas cuotas, LoginResult loginResult);
        Task<string> MorasActualizar(Moras moras, LoginResult loginResult);
        Task<string> RegistrarFormatoImpreso(int Ident_Contratos, string ContratosFormato, LoginResult loginResult);
        Task<string> ObtenerFormato(int Ident_Contratos);
        Task<List<Involucrados>> ObtenerInvolucrados(int Ident_Contratos);
        Task RecalculoMoras(int Ident_Kardex);
        Task<int> MoraExiste(int Ident_Cuotas);
        Task<Moras> ObtenerDatosMora(int Ident_Moras);
        Task<int> InsertarMoras(int Ident_Cuotas, LoginResult loginResult);
        Task MorasEliminar(int Ident_Cuotas);
        Task MorasMasivo(int Ident_Kardex);
        Task<string> AnularContrato(AnularViewModel anularViewModel, LoginResult loginResult);
        Task<string> ObtenerInvolucradosCabecera(int Ident_Contratos);
        bool FormatoVenta_Existe(int Ident_Contratos);
        Task<Ventas> FormatoVentas_List(int Ident_Contratos);
        Task FormatoVentas_Update(Ventas ventas, LoginResult loginResult);
        Task FormatoTransferencias_Update(Transferencias transferencias, LoginResult loginResult);
        Task<int> DefinirFormato(int Ident_Contratos);
        Task<Transferencias> FormatoTransferencias_List(int Ident_Contratos);
        bool FormatoTransferencia_Existe(int Ident_Contratos);
        Task FormatoTransferencias_Insert(int Ident_Contratos, LoginResult loginResult);
        Task<IngresosModel> IngresosCabecera(int Ident_Contratos);
        Task<decimal> MorasMasivo_Total(int Ident_Kardex);
    }
}
