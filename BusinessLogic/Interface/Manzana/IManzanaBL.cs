using Domain.Manzana;

namespace BusinessLogic.Interface.Manzana
{
    public interface IManzanaBL
    {
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConContratoOpciones(int nIdent_Programa);
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConSeparacionesOpciones(int nIdent_Programa);
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaLibreOpciones(int nIdent_Programa);
        Task<IEnumerable<ManzanaOpcionesDTO>> ManzanaConCartaNotarialOpciones(int nIdent_Programa);
    }
}
