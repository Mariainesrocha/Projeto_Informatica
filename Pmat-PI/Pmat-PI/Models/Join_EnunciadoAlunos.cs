using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pmat_PI.Models
{
    public class Join_EnunciadoAlunos
    {
        public int Id { get; set; }
        public int idEquipa { get; set; }
        public int? ultimoNivel { get; set; }
        public string tempo { get; set; }
        public string userId { get; set; }
        public string name { get; set; }

        public Join_EnunciadoAlunos(int Id, int idEquipa, int? ultimoNivel, string tempo, string userId, string name)
        {
            this.Id = Id;
            this.idEquipa = idEquipa;
            this.ultimoNivel = ultimoNivel;
            this.tempo = tempo;
            this.userId = userId;
            this.name = name;
        }    
    }
}
