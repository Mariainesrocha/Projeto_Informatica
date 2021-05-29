using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ModeloVelho
    {
        public int IdModel { get; set; }
        public string Name { get; set; }
        public string Objectives { get; set; }
        public string Question { get; set; }
        public string Solution { get; set; }
        public string Restrictions { get; set; }
        public string Obs { get; set; }
        public int? NumeroRespostas { get; set; }
        public DateTime? DataModificado { get; set; }
        public string ModificadoPor { get; set; }
        public string Letras { get; set; }
        public int? Tipo { get; set; }
        public int? CCicloEnsino { get; set; }
        public int? CNivelDificuldade { get; set; }
        public int? CContador { get; set; }
        public int? CResponsavel { get; set; }
        public DateTime? CDataElaboracao { get; set; }
        public string CInformacaoAdicional { get; set; }

        public virtual Modelo IdModelNavigation { get; set; }
    }
}
