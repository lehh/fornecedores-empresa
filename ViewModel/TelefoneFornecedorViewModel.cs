using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FornecedoresEmpresa.ViewModel
{
    public class TelefoneFornecedorViewModel
    {
        [Required(ErrorMessage = "Preencha o número de telefone")]
        [Display(Name = "Número")]
        public string Numero { get; set; }
    }

    public class TelefoneFornecedorPartialViewModel : TelefoneFornecedorViewModel
    {
        public string NomeLista { get; set; }

        public int Indice { get; set; }

        public string DivId { get; set; }
    }
}
