using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FornecedoresEmpresa.Models
{
    public class Fornecedor : Pessoa
    {
        public virtual ISet<TelefoneFornecedor> Telefones { get; set; }

        public virtual Empresa Empresa { get; set; }
    }
}
