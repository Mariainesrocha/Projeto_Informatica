using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Equipa
    {
        public Equipa()
        {
            EquipaAlunos = new HashSet<EquipaAluno>();
            EquipaProvas = new HashSet<EquipaProva>();
            ProvaEquipaEnunciados = new HashSet<ProvaEquipaEnunciado>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int? IdEscola { get; set; }

        public virtual Escola IdEscolaNavigation { get; set; }
        public virtual ICollection<EquipaAluno> EquipaAlunos { get; set; }
        public virtual ICollection<EquipaProva> EquipaProvas { get; set; }
        public virtual ICollection<ProvaEquipaEnunciado> ProvaEquipaEnunciados { get; set; }
    }
}
