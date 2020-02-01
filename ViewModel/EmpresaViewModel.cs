using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.ViewModel
{
    public abstract class EmpresaViewModel
    {
        [Required(ErrorMessage = "Preencha o campo Uf")]
        [Display(Name = "UF")]
        public string Uf { get; set; }

        [Required(ErrorMessage = "Preencha o Nome Fantasia")]
        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "Preencha o Cnpj")]
        [Display(Name = "CNPJ")]
        [StringLength(14)]
        public string Cnpj { get; set; }
    }

    public class EmpresaViewModelCadastro : EmpresaViewModel
    {
        
    }

    public class EmpresaViewModelDetalhes : EmpresaViewModel
    {
        public EmpresaViewModelDetalhes()
        {
            ListaFornecedor = new List<Fornecedor>();
        }

        public DateTime DataCadastro { get; set; }

        public List<Fornecedor> ListaFornecedor { get; set; }
    }

    public class EmpresaViewModelAlterar : EmpresaViewModel
    {
        public int Id { get; set; }
    }
}
