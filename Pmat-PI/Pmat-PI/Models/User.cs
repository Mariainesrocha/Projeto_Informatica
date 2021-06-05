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
        /**
        [Required(ErrorMessage = "Enter a valid date.")]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }
        **/

        [Required]
        //Question mark is nullable may be removed
        public RolesEnum Roles { get; set; }

    }
}
