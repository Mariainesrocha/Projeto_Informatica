using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ModeloNovo
    {
        public int IdModel { get; set; }
        public string Question { get; set; }
        public int IdModeLevel { get; set; }
        public int IdModelType { get; set; }
        public int IdTree { get; set; }
        public short? AnswersNumber { get; set; }
        public int? IdCycle { get; set; }
        public int? IdModelVersion { get; set; }
        public bool? Status { get; set; }
        public string Xml { get; set; }
        public int? IdUser { get; set; }
        public string Obs { get; set; }

        public virtual Modelo IdModelNavigation { get; set; }
    }
}
