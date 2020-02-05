using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FornecedoresEmpresa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FornecedoresEmpresa.Data.Persistencia;

namespace FornecedoresEmpresa.ViewModel
{
    public abstract class EmpresaViewModel
    {
        public EmpresaViewModel()
        {
            ListaIdFornecedor = new List<int>();
            SelectListaFornecedor = new List<SelectListItem>();
        }

        [Required(ErrorMessage = "Preencha o campo Uf")]
        [Display(Name = "UF")]
        [StringLength(2)]
        public string Uf { get; set; }

        [Required(ErrorMessage = "Preencha o Nome Fantasia")]
        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "Preencha o Cnpj")]
        [Display(Name = "CNPJ")]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [Display(Name = "Fornecedores")]
        public List<int> ListaIdFornecedor { get; set; }

        public List<SelectListItem> SelectListaFornecedor { get; set; }
    }

    public class EmpresaViewModelCadastro : EmpresaViewModel
    {
        
    }

    public class EmpresaViewModelDetalhes : EmpresaViewModel
    {
        public DateTime DataCadastro { get; set; }

        public List<Fornecedor> ListaFornecedor { get; set; }
    }

    public class EmpresaViewModelAlterar : EmpresaViewModel
    {
        public int Id { get; set; }
    }
}
