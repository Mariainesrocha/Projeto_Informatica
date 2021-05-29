using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Projeto
    {
        public Projeto()
        {
            UserEscolaHistoricos = new HashSet<UserEscolaHistorico>();
            UserEscolas = new HashSet<UserEscola>();
        }

        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }

        public virtual ICollection<UserEscolaHistorico> UserEscolaHistoricos { get; set; }
        public virtual ICollection<UserEscola> UserEscolas { get; set; }
    }
}
