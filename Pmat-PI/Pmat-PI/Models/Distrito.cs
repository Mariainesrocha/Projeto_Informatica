using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Distrito
    {
        public Distrito()
        {
            Concelhos = new HashSet<Concelho>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int? Pais { get; set; }

        public virtual Pai PaisNavigation { get; set; }
        public virtual ICollection<Concelho> Concelhos { get; set; }
    }
}
