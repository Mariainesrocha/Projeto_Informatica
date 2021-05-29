using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Freguesium
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? Concelho { get; set; }

        public virtual Concelho ConcelhoNavigation { get; set; }
    }
}
