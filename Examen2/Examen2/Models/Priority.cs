using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2.Models
{
    public class Priority
    {
        [Key]

        public int IdPriority { get; set; }


        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(20, ErrorMessage = "No debe de tener mas de 20 caracteres")]
        [Display(Name = "Descripcion")]
        [MinLength(2, ErrorMessage = "Debe de tener mas de 2 caracteres")]
        public string PriorityDescription { get; set; }

        public IEnumerable<Homework> Homeworks { get; set; }
    }
}
