using BusinessLogic.Interface.Programa;
using Domain.Programa;
using Repository.Interface.Programa;

namespace BusinessLogic.Programa
{
    public class ProgramaBL: IProgramaBL
    {
        IProgramaRepository _programaRepository;
        public ProgramaBL(IProgramaRepository programaRepository)
        {
            _programaRepository = programaRepository;
        }
        public async Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaOpciones()
        { 
            return await _programaRepository.ProgramaOpciones();    
        }
        public async Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConCartaNotarial()
        { 
            return await _programaRepository.ProgramaConCartaNotarial();
        }
        public async Task<IEnumerable<ProgramaOpcionesDTO>> ProgramaConContrato()
        {
            return await _programaRepository.ProgramaConContrato();
        }
    }
}
