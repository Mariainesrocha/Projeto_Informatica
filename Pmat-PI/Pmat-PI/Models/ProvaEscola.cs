using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ProvaEscola
    {
        public int IdEscola { get; set; }
        public int IdProva { get; set; }
        public int? IdUserEscola { get; set; }
        public DateTime DataRegisto { get; set; }
        public int? EscolaOrganizadora { get; set; }
        public string AnoLetivo { get; set; }

        public virtual AnoLetivo AnoLetivoNavigation { get; set; }
        public virtual Escola EscolaOrganizadoraNavigation { get; set; }
        public virtual Escola IdEscolaNavigation { get; set; }
        public virtual Prova IdProvaNavigation { get; set; }
    }
}
