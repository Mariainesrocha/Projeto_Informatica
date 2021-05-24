using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Pmat_PI.Models
{
    public class EmailModel2
    {
        [Required, Display(Name = "Grupo Destino"), RequiredRoleOrEmail(ErrorMessage ="Must either be a valid group of users or an email.") ]
        public string Destino { get; set; }

        [Required, Display(Name = "Assunto")]
        public string Assunto { get; set; }
        [Required, Display(Name = "Mensagem")]
        public string Mensagem { get; set; }

        [Display(Name = "Ficheiro")]
        public IFormFile Ficheiro { get; set; }


    }


    public enum RoleEnum
    {
        Admin, Professor, Aluno
    }


    public class RequiredRoleOrEmail : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            Console.WriteLine("value:" + value);
            return Enum.IsDefined(typeof(RoleEnum), value) || IsValidEmail(value.ToString());
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
