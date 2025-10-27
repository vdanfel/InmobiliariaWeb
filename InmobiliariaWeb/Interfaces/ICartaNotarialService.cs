using InmobiliariaWeb.Models.CartaNotarial;
using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Separaciones;

namespace InmobiliariaWeb.Interfaces
{
    public interface ICartaNotarialService
    {
        Task<List<ManzanaCbxList>> ManzanaCbxListar(int ident_Programa);
        Task<List<LoteCbxList>> LoteCbxListar(int ident_Manzana);
        Task<ContratosDTO> ObtenerContratoPorLote(int nIdent_Lote);
        Task<List<ClientesListCbxDTO>> ListarClientesPorContrato(int nIdent_Contrato);
        Task<int> CartaNotarialCreate(CartaNotarialDTO cartaNotarialDTO);
    }
}
