using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Concelho
    {
        public Concelho()
        {
            Escolas = new HashSet<Escola>();
            Freguesia = new HashSet<Freguesium>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int? Distrito { get; set; }

        public virtual Distrito DistritoNavigation { get; set; }
        public virtual ICollection<Escola> Escolas { get; set; }
        public virtual ICollection<Freguesium> Freguesia { get; set; }
    }
}
