using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FornecedoresEmpresa.Models;
using ExpressiveAnnotations.Attributes;

namespace FornecedoresEmpresa.ViewModel
{
    public abstract class FornecedorViewModel
    {
        public FornecedorViewModel()
        {
            ListaTelefoneFornecedor = new List<TelefoneFornecedorPartialViewModel>();
        }

        [Required(ErrorMessage = "Preencha o Nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [RequiredIf("Cnpj == null", ErrorMessage = "Preencha o CPF")]
        [Display(Name = "CPF")]
        [StringLength(11)]
        public string Cpf { get; set; }

        [RequiredIf("Cpf == null", ErrorMessage = "Preencha o CNPJ")]
        [AssertThat("Cpf != null ? Cnpj == null : true", ErrorMessage = "Apenas o CNPJ ou CPF deve ser preenchido")]
        [Display(Name = "CNPJ")]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [RequiredIf("Cpf != null", ErrorMessage = "Preencha o RG")]
        [AssertThat("Cnpj != null ? Rg == null : true", ErrorMessage = "Pessoa jurídica não precisa informar o RG")]
        [Display(Name = "RG")]
        [StringLength(14)]
        public string Rg { get; set; }

        [RequiredIf("Cpf != null", ErrorMessage = "Preencha a Data de Nascimento")]
        [AssertThat("Cnpj != null ? DataNascimento == null : true", ErrorMessage = "Pessoa jurídica não precisa informar data de nascimento")]
        [Display(Name = "Data de Nascimento")]
        public Nullable<DateTime> DataNascimento { get; set; }

        public List<TelefoneFornecedorPartialViewModel> ListaTelefoneFornecedor { get; set; }
    }

    public class FornecedorViewModelCadastro : FornecedorViewModel
    {

    }
}
