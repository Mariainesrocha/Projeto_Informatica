using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ProvaModelo
    {
        public int IdProva { get; set; }
        public int IdModelo { get; set; }
        public int Nivel { get; set; }

        public virtual Modelo IdModeloNavigation { get; set; }
        public virtual Prova IdProvaNavigation { get; set; }
    }
}
