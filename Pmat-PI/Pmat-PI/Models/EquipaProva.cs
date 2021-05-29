using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class EquipaProva
    {
        public int IdEquipa { get; set; }
        public int IdProva { get; set; }

        public virtual Equipa IdEquipaNavigation { get; set; }
        public virtual Prova IdProvaNavigation { get; set; }
    }
}
