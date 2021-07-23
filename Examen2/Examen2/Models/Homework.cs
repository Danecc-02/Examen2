using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2.Models
{
    public class Homework
    {
        [Key]
        public int IdHomework { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(70, ErrorMessage = "No debe de tener mas de 70 caracteres")]
        [Display(Name = "Descripcion")]
        [MinLength(2, ErrorMessage = "Debe de tener mas de 2 caracteres")]
        public string Description { get; set; }

        
        [Display(Name = "Prioridad")]
        public string Priority { get; set; }

        public int IdCategory { get; set; }

        [ForeignKey("IdCategory")]
       
        [Display(Name = "Categoria")]
        public Category Category { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de creacion")]
        
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha limite")]
        
        public DateTime FinishDate { get; set; }



    }
}
