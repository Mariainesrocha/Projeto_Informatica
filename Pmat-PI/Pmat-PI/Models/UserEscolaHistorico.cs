using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class UserEscolaHistorico
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public int IdEscola { get; set; }
        public int? IdResponsavel { get; set; }
        public int? IdAnoEscolar { get; set; }
        public string AnoLetivo { get; set; }
        public int? IdProjeto { get; set; }
        public DateTime? Data { get; set; }

        public virtual AnoLetivo AnoLetivoNavigation { get; set; }
        public virtual AnoEscolar IdAnoEscolarNavigation { get; set; }
        public virtual Escola IdEscolaNavigation { get; set; }
        public virtual Projeto IdProjetoNavigation { get; set; }
        public virtual UserEscola IdResponsavelNavigation { get; set; }
        public virtual AspNetUser IdUserNavigation { get; set; }
    }
}
