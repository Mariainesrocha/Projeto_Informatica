using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class AnoLetivo
    {
        public AnoLetivo()
        {
            ProvaEscolas = new HashSet<ProvaEscola>();
        }

        public string AnoLetivo1 { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

        public virtual ICollection<ProvaEscola> ProvaEscolas { get; set; }
    }
}
