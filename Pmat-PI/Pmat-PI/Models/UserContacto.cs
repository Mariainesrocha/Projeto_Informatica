using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class UserContacto
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdTipo { get; set; }
        public string Descricao { get; set; }
    }
}
