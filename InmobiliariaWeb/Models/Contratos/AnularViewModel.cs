using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Models.Contratos
{
    public class AnularViewModel
    {
        public int MotivoAnulacion { get; set; }
        public string DetalleAnulacion { get; set; }
        public ViewModels.Contratos.ActualizarViewModel Actualizar { get; set; }
        public List<ClienteList> Clientes { get; set; }
    }
}
