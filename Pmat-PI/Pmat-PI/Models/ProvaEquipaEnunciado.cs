using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ProvaEquipaEnunciado
    {
        public int Id { get; set; }
        public int IdEquipa { get; set; }
        public int IdProva { get; set; }
        public DateTime Data { get; set; }
        public int? Status { get; set; }
        public int? UltimoNivel { get; set; }
        public string Tempo { get; set; }

        public virtual Equipa IdEquipaNavigation { get; set; }
        public virtual Prova IdProvaNavigation { get; set; }
    }
}
