using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Competicao
    {
        public Competicao()
        {
            Provas = new HashSet<Prova>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Nome da competição deve ter pelo menos 3 caracteres")]
        [StringLength(255, ErrorMessage = "Nome da competição não pode exceder 255 caracteres")]
        public string Nome { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Etiqueta deve ter pelo menos 3 caracteres")]
        [StringLength(50, ErrorMessage = "Etiqueta não pode exceder 50 caracteres")]
        public string Etiqueta { get; set; }

        public virtual ICollection<Prova> Provas { get; set; }
    }
}
