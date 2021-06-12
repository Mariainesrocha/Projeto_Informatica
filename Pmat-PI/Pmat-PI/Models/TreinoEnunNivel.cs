using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class TreinoEnunNivel
    {
        public TreinoEnunNivel()
        {
            TreinoEnunNivelUserResps = new HashSet<TreinoEnunNivelUserResp>();
        }

        public int IdEnunciadoEquipa { get; set; }
        public int IdNivel { get; set; }
        public int IdModel { get; set; }
        public string PerguntaMathMl { get; set; }
        public string Resp1 { get; set; }
        public string Resp2 { get; set; }
        public string Resp3 { get; set; }
        public string Resp4 { get; set; }
        public bool Sol1 { get; set; }
        public bool Sol2 { get; set; }
        public bool Sol3 { get; set; }
        public bool Sol4 { get; set; }
        public string OperadorPergunta { get; set; }
        public string OperadorResp1 { get; set; }
        public string OperadorResp2 { get; set; }
        public string OperadorResp3 { get; set; }
        public string OperadorResp4 { get; set; }
        public string ParametroPergunta { get; set; }
        public string ParametroResp1 { get; set; }
        public string ParametroResp2 { get; set; }
        public string ParametroResp3 { get; set; }
        public string ParametroResp4 { get; set; }
        public string Obs1 { get; set; }
        public string Obs2 { get; set; }
        public string Obs3 { get; set; }
        public string Obs4 { get; set; }

        public virtual TreinoEnunciado IdEnunciadoEquipaNavigation { get; set; }
        public virtual ICollection<TreinoEnunNivelUserResp> TreinoEnunNivelUserResps { get; set; }
    }
}
