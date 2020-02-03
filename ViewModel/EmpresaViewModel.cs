using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FornecedoresEmpresa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FornecedoresEmpresa.ViewModel
{
    public abstract class EmpresaViewModel
    {
        public EmpresaViewModel()
        {
            ListaFornecedor = new List<Fornecedor>();
            ListaIdFornecedor = new List<int>();
        }

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

        public List<int> ListaIdFornecedor { get; set; }

        public List<Fornecedor> ListaFornecedor { get; set; }
    }

    public class EmpresaViewModelCadastro : EmpresaViewModel
    {
        
    }

    public class EmpresaViewModelDetalhes : EmpresaViewModel
    {
        public DateTime DataCadastro { get; set; }
    }

    public class EmpresaViewModelAlterar : EmpresaViewModel
    {
        public int Id { get; set; }
    }
}
