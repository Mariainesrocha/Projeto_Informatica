using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pmat_PI.Models
{
    public class ViewModel_ProvaDetails
    {
        public Prova prova { get; set; }
        public List<Join_EnunciadoAlunos> enunciados { get; set; }
    }
}
