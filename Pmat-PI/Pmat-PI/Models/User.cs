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

        [MaxLength(4, ErrorMessage = "Codigo Postal first half can't exceed 4 chars")]
        public string CodPostal { get; set; }

        [MaxLength(3, ErrorMessage = "Codigo Postal second half can't exceed 3 chars")]
        public string ExtensaoCodPostal { get; set; }

        [MaxLength(30, ErrorMessage = "Localidade can't exceed 30 chars")]
        public string Localidade { get; set; }
    }
}
