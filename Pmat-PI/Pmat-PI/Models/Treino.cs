using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Treino
    {
        public Treino()
        {
            TreinoEnunciados = new HashSet<TreinoEnunciado>();
            TreinoModelos = new HashSet<TreinoModelo>();
        }

        [Required]
        public int Id { get; set; }
        public string IdAuthor { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Nome da prova deve ter pelo menos 3 caracteres")]
        [StringLength(60, ErrorMessage = "Nome da prova não pode exceder 60 caracteres")]
        public string NomeProva { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int? MaxEscolas { get; set; }
        public int? MaxTentJogo { get; set; }
        public int? TempoTotalJogo { get; set; }

        [Required]
        public int NumNiveis { get; set; }
        public int? VidasPorNivel { get; set; }
        public int? NumElemsEquipa { get; set; }
        public bool? Calculadora { get; set; }

        [StringLength(25, ErrorMessage = "Nome da prova não pode exceder 25 caracteres")]
        public string Estilo { get; set; }

        [Url(ErrorMessage = "URL inválido. Exemplo: http://www.exemplo.com")]
        public string Url { get; set; }

        [Required]
        public bool TreinoVisivel { get; set; }
        public int? RefIdCicloEnsino { get; set; }
        public int? Plataforma { get; set; }

        public virtual AspNetUser IdAuthorNavigation { get; set; }
        public virtual CicloEnsino RefIdCicloEnsinoNavigation { get; set; }
        public virtual ICollection<TreinoEnunciado> TreinoEnunciados { get; set; }
        public virtual ICollection<TreinoModelo> TreinoModelos { get; set; }
    }
}
