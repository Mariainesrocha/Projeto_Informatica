using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            EquipaAlunos = new HashSet<EquipaAluno>();
            Provas = new HashSet<Prova>();
            TreinoEnunciados = new HashSet<TreinoEnunciado>();
            Treinos = new HashSet<Treino>();
            UserEscolaHistoricos = new HashSet<UserEscolaHistorico>();
            UserEscolas = new HashSet<UserEscola>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EquipaAluno> EquipaAlunos { get; set; }
        public virtual ICollection<Prova> Provas { get; set; }
        public virtual ICollection<TreinoEnunciado> TreinoEnunciados { get; set; }
        public virtual ICollection<Treino> Treinos { get; set; }
        public virtual ICollection<UserEscolaHistorico> UserEscolaHistoricos { get; set; }
        public virtual ICollection<UserEscola> UserEscolas { get; set; }
    }

}
