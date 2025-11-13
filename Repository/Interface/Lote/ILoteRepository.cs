using Domain.Lote;

namespace Repository.Interface.Lote
{
    public interface ILoteRepository
    {
        Task<IEnumerable<LoteOpcionesDTO>> LoteLibreOpciones(int nIdent_Manzana);
        Task<IEnumerable<LoteOpcionesDTO>> LoteConContratoOpciones(int nIdent_Manzana);
        Task<IEnumerable<LoteOpcionesDTO>> LoteConSeparacionOpciones(int nIdent_Manzana);
        Task<IEnumerable<LoteOpcionesDTO>> LoteConCartaNotarialOpciones(int nIdent_Manzana);
    }
}
