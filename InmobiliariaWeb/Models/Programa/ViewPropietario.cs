using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result.Programa;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaWeb.Models.Programa
{
    public class ViewPropietario
    {
        
        public int IdentPrograma {  get; set; }
        public string NombrePrograma { get; set; }
        [Required (ErrorMessage = "Tiene que escoger a un Propietario")]
        public int IdentPersona { get; set; }
        public string Mensaje { get; set; }
        [Required(ErrorMessage ="Debe escoger un Tipo de Propietario")]
        public int Ident011TipoPropietario { get; set; }
        public string NumeroPartida { get; set; }
        public List<PropietarioList> PropietarioList { get; set; }
        public List<TipoPropietario> TipoPropietario { get; set; }
    }
}
