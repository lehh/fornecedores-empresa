using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.ViewModel
{
    public abstract class FornecedorViewModel
    {
        [Required(ErrorMessage = "Preencha o campo Nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o campo CPF")]
        [Display(Name = "CPF")]
        [StringLength(11)]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Preencha o campo CNPJ")]
        [Display(Name = "CNPJ")]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "Preencha o campo Data de Nascimento")]
        [Display(Name = "Data de Nascimento")]
        public Nullable<DateTime> DataNascimento { get; set; }
    }

    public class FornecedorViewModelCadastro : FornecedorViewModel
    {

    }
}
