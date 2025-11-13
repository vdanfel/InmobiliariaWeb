using Domain.Programa;

namespace BusinessLogic.Interface.Programa
{
    public interface IProgramaBL
    {
        Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaOpciones();
        Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConCartaNotarial();
        Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConContrato();
    }
}
