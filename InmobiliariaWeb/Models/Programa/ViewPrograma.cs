﻿using InmobiliariaWeb.Models.Tablas;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaWeb.Models.Programa
{
    public class ViewPrograma
    {
        public int IdentPrograma { get; set; }
        [Required (ErrorMessage = "El Nombre de Programa es obligatorio")]
        public string NombrePrograma { get; set; }
        public string Codigo { get; set; }
        public string NumeroPartida { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        [Required(ErrorMessage ="El Area Total es obligatorio")]
        public decimal AreaTotal { get; set; }
        [Required(ErrorMessage ="El Area Lotizada es obligatoria")]
        public decimal AreaLotizada { get; set; }
        [Required(ErrorMessage ="La Cantidad de Manzanas es obligatoria")]
        [Range (1,50)]
        public int CantidadManzanas { get; set; }
        public string Suministro { get; set; }
        public string Mensaje { get; set; }
        public List<TipoPropietario>TipoPropietario { get; set; }
        public List<ViewPropietario> viewPropietarios { get; set; }
        public List<ViewManzana> viewManzana { get; set;}
        public List<Manzanas> manzanas { get; set;}
        public int ManzanaInicial { get; set; }
        public string Confirmacion { get; set; }
        public bool Ident_012_EstadoLote { get; set; }
        public int Ident_017_TipoContrato { get; set; }
        public List<TipoContrato> TipoContratos { get; set; }
        public string Clausula1 { get; set; }
        public decimal PorcentajeLiquidacion { get; set; }
    }
}
