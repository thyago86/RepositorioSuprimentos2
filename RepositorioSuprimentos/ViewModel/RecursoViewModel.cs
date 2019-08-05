using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorioSuprimentos.ViewModel {
    public class RecursoViewModel {
        [Required]
        public string Descricao { get; set; }
        [Required]
        public int Quantidade { get; set; }
        [Required]
        public string Observacao { get; set; }
    }
}
