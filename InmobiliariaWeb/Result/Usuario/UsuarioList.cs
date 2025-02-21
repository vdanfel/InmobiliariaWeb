using InmobiliariaWeb.Models.Tablas;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaWeb.Result.Usuario
{
    public class UsuarioList
    {
        public int Indice { get; set; }
        public int Ident_Usuario { get; set; }
        [Required]
        public int IdentPersona { get; set; }
        public string Nombre { get; set; }
        public string Documento { get; set; }
        [Required (ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Usuario { get; set; }
        public string Clave { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Clave1 { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Clave2 { get; set; }
        [Required(ErrorMessage = "Debe escoger un Tipo de Usuario")]
        public int Ident_005_TipoUsuario { get; set; }
        public string TipoUsuario { get; set; }
        public string Mensaje { get; set; }
        public List<TipoRol>TipoRols { get; set; }
        
    }
}
