using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ProvaEqEnunNivelUserResp
    {
        public int IdEnunciadoEquipa { get; set; }
        public int IdNivel { get; set; }
        public int Tentativa { get; set; }
        public int? RespDada1 { get; set; }
        public int RespDada2 { get; set; }
        public int? RespDada3 { get; set; }
        public int? RespDada4 { get; set; }
        public string Tempo { get; set; }
        public DateTime? Data { get; set; }

        public virtual ProvaEqEnunNivel Id { get; set; }
    }
}
