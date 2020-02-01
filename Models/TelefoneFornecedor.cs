using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FornecedoresEmpresa.Models
{
    public class TelefoneFornecedor
    {
        public virtual int Id { get; set; }

        public virtual string Numero { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }
    }
}
