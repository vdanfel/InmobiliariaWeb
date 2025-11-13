using Domain.Programa;

namespace Repository.Interface.Programa
{
    public interface IProgramaRepository
    {
        Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaOpciones();
        Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConCartaNotarial();
        Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConContrato();
    }
}
