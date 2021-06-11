using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pmat_PI.Models
{
    public class User : IdentityUser
    {
        [Required, MaxLength(80, ErrorMessage = "Name can't exceed 80 chars")]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [MaxLength(100, ErrorMessage = "Morada can't exceed 80 chars")]
        public string Morada { get; set; }

        public int sexo { get; set; }
            
        public DateTime? DataRegisto { get; set; }
        public string CodPostal { get; set; }
        public string ExtensaoCodPostal { get; set; }
        public string Localidade { get; set; }
    }
}
