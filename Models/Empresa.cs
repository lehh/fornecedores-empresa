using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FornecedoresEmpresa.Models
{
    public class Empresa
    {
        public virtual int Id { get; set; }

        public virtual string Uf { get; set; }

        public virtual string NomeFantasia { get; set; }

        public virtual string Cnpj { get; set; }

        public virtual DateTime DataCadastro { get; set; }

        public virtual Nullable<DateTime> DataModificacao { get; set; }

        public virtual ISet<Fornecedor> Fornecedores { get; set; }
    }
}
