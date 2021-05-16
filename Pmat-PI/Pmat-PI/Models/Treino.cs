using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Treino
    {
        public int Id { get; set; }
        public string IdAuthor { get; set; }
        public int? IdCompeticao { get; set; }
        public string NomeProva { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int? MaxEscolas { get; set; }
        public int? MaxTentJogo { get; set; }
        public int? TempoTotalJogo { get; set; }
        public int NumNiveis { get; set; }
        public int? VidasPorNivel { get; set; }
        public int? NumElemsEquipa { get; set; }
        public bool? Calculadora { get; set; }
        public string Estilo { get; set; }
        public string Url { get; set; }
        public bool TreinoVisivel { get; set; }
        public int? RefIdCicloEnsino { get; set; }
        public int? Plataforma { get; set; }

        public virtual AspNetUser IdAuthorNavigation { get; set; }
        public virtual Competicao IdCompeticaoNavigation { get; set; }
    }
}
