using InmobiliariaWeb.Models.Documentos;
using InmobiliariaWeb.Models.Opciones;
using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Interfaces
{
    public interface IRutaService
    {
        Task<int> RutaArchivoCreate(RutaArchivoDTO rutaArchivoDTO);
        Task<List<RutaArchivoDTO>> RutaArchivoList(RutaArchivoRequestDTO rutaArchivoRequestDTO);
        Task<List<FormatosResponseDTO>> FormatosList(int nIdent_Contratos);
        Task<string> FormatoDownload(FormatoSelectDTO formatoSelectDTO);
        Task<int> RutaArchivoDelete(RutaArchivoDTO rutaArchivoDTO);
        Task<IEnumerable<TipoArchivoOpcionDTO>> TipoArchivoOpcionListar();
    }
}
