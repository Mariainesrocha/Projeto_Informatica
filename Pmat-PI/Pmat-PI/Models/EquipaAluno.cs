using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class EquipaAluno
    {
        public int IdAlunoEquipa { get; set; }
        public int? IdEquipa { get; set; }
        public string IdUser { get; set; }

        public virtual Equipa IdEquipaNavigation { get; set; }
        public virtual AspNetUser IdUserNavigation { get; set; }
    }
}
