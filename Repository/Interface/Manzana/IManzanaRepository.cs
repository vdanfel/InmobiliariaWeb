using Domain.Manzana;

namespace Repository.Interface.Manzana
{
    public interface IManzanaRepository
    {
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConContratoOpciones(int nIdent_Programa);
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConSeparacionesOpciones(int nIdent_Programa);
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaLibreOpciones(int nIdent_Programa);
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConCartaNotarialOpciones(int nIdent_Programa);
    }
}
