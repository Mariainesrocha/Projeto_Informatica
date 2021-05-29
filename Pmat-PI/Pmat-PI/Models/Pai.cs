using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Pai
    {
        public Pai()
        {
            Distritos = new HashSet<Distrito>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Distrito> Distritos { get; set; }
    }
}
