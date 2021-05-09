using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class TipoEscola
    {
        public TipoEscola()
        {
            Escolas = new HashSet<Escola>();
        }

        public int IdTipoEscola { get; set; }
        public string TipoEscola1 { get; set; }
        public string DescricaoTipoEscola { get; set; }

        public virtual ICollection<Escola> Escolas { get; set; }
    }
}
