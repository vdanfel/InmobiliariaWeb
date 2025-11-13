using Domain.Lote;

namespace BusinessLogic.Interface.Lote
{
    public interface ILoteBL
    {
        Task<IEnumerable<LoteOpcionesDTO>> LoteLibreOpciones(int nIdent_Manzana);
        Task<IEnumerable<LoteOpcionesDTO>> LoteConContratoOpciones(int nIdent_Manzana);
        Task<IEnumerable<LoteOpcionesDTO>> LoteConSeparacionOpciones(int nIdent_Manzana);
        Task<IEnumerable<LoteOpcionesDTO>> LoteConCartaNotarialOpciones(int nIdent_Manzana);
    }
}
