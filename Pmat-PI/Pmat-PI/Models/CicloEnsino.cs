using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class CicloEnsino
    {
        public CicloEnsino()
        {
            Provas = new HashSet<Prova>();
            Treinos = new HashSet<Treino>();
        }

        public int Id { get; set; }
        public string Descritivo { get; set; }
        public string Abreviatura { get; set; }

        public virtual ICollection<Prova> Provas { get; set; }
        public virtual ICollection<Treino> Treinos { get; set; }
    }
}
