using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class TreinoEnunciado
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public int IdTreino { get; set; }
        public DateTime Data { get; set; }
        public int? Status { get; set; }
        public int? UltimoNivel { get; set; }
        public string Tempo { get; set; }

        public virtual Treino IdTreinoNavigation { get; set; }
        public virtual AspNetUser IdUserNavigation { get; set; }
    }
}
