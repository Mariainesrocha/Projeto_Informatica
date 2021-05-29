using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class TreinoModelo
    {
        public int IdTreino { get; set; }
        public int IdModelo { get; set; }
        public int Nivel { get; set; }

        public virtual Modelo IdModeloNavigation { get; set; }
        public virtual Treino IdTreinoNavigation { get; set; }
    }
}
