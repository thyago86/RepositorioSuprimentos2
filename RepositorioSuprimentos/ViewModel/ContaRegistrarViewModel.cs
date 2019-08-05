using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RepositorioSuprimentos.ViewModel {
    public class ContaRegistrarViewModel {
        [Required]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
