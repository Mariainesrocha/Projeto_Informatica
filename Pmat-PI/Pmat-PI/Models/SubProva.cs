using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class SubProva
    {
        public int IdProvaPai { get; set; }
        public int IdProvaFilho { get; set; }

        public virtual Prova IdProvaFilhoNavigation { get; set; }
        public virtual Prova IdProvaPaiNavigation { get; set; }
    }
}
