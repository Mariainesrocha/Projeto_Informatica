using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Escola
    {
        public int Id { get; set; }
        public int IdTipoEscola { get; set; }
        public string NomeEscola { get; set; }
        public string Morada { get; set; }
        public string CodigoPostal { get; set; }
        public string ExtensaoCodPostal { get; set; }
        public string Localidade { get; set; }
        public string Telefone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int? Idconcelho { get; set; }
        public int? IdFreguesia { get; set; }
        public bool? Estado { get; set; }
        public string Ensinos { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Gruponatureza { get; set; }
        public int? CodDgeec { get; set; }
        public int? CodDgpgf { get; set; }
    }
}
