using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class AnoEscolar
    {
        public AnoEscolar()
        {
            UserEscolaHistoricos = new HashSet<UserEscolaHistorico>();
            UserEscolas = new HashSet<UserEscola>();
        }

        public int Id { get; set; }
        public string Ano { get; set; }

        public virtual ICollection<UserEscolaHistorico> UserEscolaHistoricos { get; set; }
        public virtual ICollection<UserEscola> UserEscolas { get; set; }
    }
}
