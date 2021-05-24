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
        [Required, Display(Name = "Grupo Destino"), EnumDataType(typeof(RoleEnum))]
        public string Destino { get; set; }
        [Required, Display(Name = "Assunto")]
        public string Assunto { get; set; }
        [Required, Display(Name = "Mensagem")]
        public string Mensagem { get; set; }

        [Display(Name ="Ficheiro")]
        public IFormFile Ficheiro { get; set; }
    }


    public enum RoleEnum
    {
       Admin,Professor,Aluno,All
    }
}
