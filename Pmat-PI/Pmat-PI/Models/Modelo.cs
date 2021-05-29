using System;
using System.Collections.Generic;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class Modelo
    {
        public Modelo()
        {
            ProvaModelos = new HashSet<ProvaModelo>();
            TreinoModelos = new HashSet<TreinoModelo>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }

        public virtual ModeloNovo ModeloNovo { get; set; }
        public virtual ModeloVelho ModeloVelho { get; set; }
        public virtual ICollection<ProvaModelo> ProvaModelos { get; set; }
        public virtual ICollection<TreinoModelo> TreinoModelos { get; set; }
    }
}
