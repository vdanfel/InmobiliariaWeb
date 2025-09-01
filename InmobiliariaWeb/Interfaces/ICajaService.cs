using InmobiliariaWeb.Models.Caja;
using InmobiliariaWeb.Models.Ingresos;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Interfaces
{
    public interface ICajaService
    {
        Task<int> Obtener_Ident_Ingresos(int Ident_021_TipoIngresos, int Ident_Origen);
        Task<List<CuentasBancariasList>> CuentasBancariasXBanco(int Ident_019_Banco);
        Task<int> Ingresos_Insert(IngresosModel ingresosModel, LoginResult loginResult);
        Task Ingresos_Update(IngresosModel ingresosModel, LoginResult loginResult);
        Task<int> IngresosDetalle_Insert(IngresosDetalleModel ingresosDetalleModel, LoginResult loginResult);
        Task<List<IngresosDetallesList>> IngresosDetalle_List(int Ident_Ingresos);
        Task IngresosDetalle_Delete(int Ident_IngresosDetalle, LoginResult loginResult);
        Task<decimal> IngresosDetalle_ImporteTotal(int Ident_Ingresos);
        Task Ingresos_ValidarImportes(int Ident_IngresosDetalle, int Ident_021_TipoIngresos);
        Task<List<IngresosIndexTablaDTO>> IngresosIndex(IngresosIndexFilterDTO ingresosIndexFilterDTO);
        Task<IngresosViewModel> IngresosSelect(int nIdent_Ingresos);
        Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa);
        Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana);
        Task IngresosActualizarTotal(int nIdent_Ingresos);
    }
}
