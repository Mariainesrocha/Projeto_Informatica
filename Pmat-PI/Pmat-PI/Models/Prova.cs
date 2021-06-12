using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Prova
    {
        public Prova()
        {
            EquipaProvas = new HashSet<EquipaProva>();
            ProvaEquipaEnunciados = new HashSet<ProvaEquipaEnunciado>();
            ProvaEscolas = new HashSet<ProvaEscola>();
            ProvaModelos = new HashSet<ProvaModelo>();
            SubProvaIdProvaFilhoNavigations = new HashSet<SubProva>();
            SubProvaIdProvaPaiNavigations = new HashSet<SubProva>();
        }

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
        public DateTime? DataInscFinal { get; set; }
        public DateTime? DataProva { get; set; }
        public DateTime? InicioPreInscricao { get; set; }
        public DateTime? FimPreInscricao { get; set; }
        public DateTime? InicioInscricaoEquipas { get; set; }
        public DateTime? FimInscricaoEquipas { get; set; }
        public DateTime? FimProva { get; set; }
        public string Estilo { get; set; }
        public string Url { get; set; }
        public bool TreinoVisivel { get; set; }
        public int? RefIdCicloEnsino { get; set; }
        public int? Plataforma { get; set; }

        public virtual AspNetUser IdAuthorNavigation { get; set; }
        public virtual Competicao IdCompeticaoNavigation { get; set; }
        public virtual CicloEnsino RefIdCicloEnsinoNavigation { get; set; }
        public virtual ICollection<EquipaProva> EquipaProvas { get; set; }
        public virtual ICollection<ProvaEquipaEnunciado> ProvaEquipaEnunciados { get; set; }
        public virtual ICollection<ProvaEscola> ProvaEscolas { get; set; }
        public virtual ICollection<ProvaModelo> ProvaModelos { get; set; }
        public virtual ICollection<SubProva> SubProvaIdProvaFilhoNavigations { get; set; }
        public virtual ICollection<SubProva> SubProvaIdProvaPaiNavigations { get; set; }
    }
}
