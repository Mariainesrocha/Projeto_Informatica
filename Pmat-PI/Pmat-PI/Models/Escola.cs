using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Escola
    {
        public Escola()
        {
            Equipas = new HashSet<Equipa>();
            ProvaEscolaEscolaOrganizadoraNavigations = new HashSet<ProvaEscola>();
            ProvaEscolaIdEscolaNavigations = new HashSet<ProvaEscola>();
            UserEscolaHistoricos = new HashSet<UserEscolaHistorico>();
            UserEscolas = new HashSet<UserEscola>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public int IdTipoEscola { get; set; }

        [Required]
        [MinLength(3,ErrorMessage = "Nome da escola deve ter pelo menos 3 caracteres")]
        [StringLength(100, ErrorMessage = "Nome da escola não pode exceder 100 caracteres")]
        public string NomeEscola { get; set; }

        [StringLength(100, ErrorMessage = "Morada não pode exceder 100 caracteres")]
        public string Morada { get; set; }

        [RegularExpression("[0-9]{4}",ErrorMessage ="Código Postal inválido, deve conter 4 algarismos")]
        public string CodigoPostal { get; set; }

        [RegularExpression("[0-9]{3}", ErrorMessage = "Extensão do Código Postal inválida, deve conter 3 algarismos")]
        public string ExtensaoCodPostal { get; set; }

        [StringLength(50, ErrorMessage = "Localidade não pode exceder 50 caracteres")]
        public string Localidade { get; set; }

        [RegularExpression("[0-9]{9}", ErrorMessage = "Número de telefone inválido, deve conter 9 algarismos")]
        public string Telefone { get; set; }

        [RegularExpression("[0-9]{9}", ErrorMessage = "Fax inválido, deve conter 9 algarismos")]
        public string Fax { get; set; }

        [Email(ErrorMessage = "Endereço de email inválido")]
        [StringLength(255, ErrorMessage = "Email não pode exceder 255 caracteres")]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Url(ErrorMessage = "URL inválido. Exemplo: http://www.exemplo.com")]
        public string Website { get; set; }

        public int? Idconcelho { get; set; }
        public int? IdFreguesia { get; set; }
        public bool? Estado { get; set; }
        public string Ensinos { get; set; }

        [RegularExpression("[-]?[0-9]*([.][0-9]*)?", ErrorMessage = "Latitude contém caracteres inválidos")]
        public string Latitude { get; set; }

        [RegularExpression("[-]?[0-9]*([.][0-9]*)?", ErrorMessage = "Longitude contém caracteres inválidos")]
        public string Longitude { get; set; }


        public string Gruponatureza { get; set; }
        public int? CodDgeec { get; set; }
        public int? CodDgpgf { get; set; }

        public virtual TipoEscola IdTipoEscolaNavigation { get; set; }
        public virtual Concelho IdconcelhoNavigation { get; set; }
        public virtual ICollection<Equipa> Equipas { get; set; }
        public virtual ICollection<ProvaEscola> ProvaEscolaEscolaOrganizadoraNavigations { get; set; }
        public virtual ICollection<ProvaEscola> ProvaEscolaIdEscolaNavigations { get; set; }
        public virtual ICollection<UserEscolaHistorico> UserEscolaHistoricos { get; set; }
        public virtual ICollection<UserEscola> UserEscolas { get; set; }
    }
}
