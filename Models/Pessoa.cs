using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FornecedoresEmpresa.Models
{
    public abstract class Pessoa
    {
        public virtual int Id { get; set; }

        public virtual string Nome { get; set; }

        public virtual string Cnpj { get; set; }

        public virtual string Cpf { get; set; }

        public virtual string Rg { get; set; }

        public virtual Nullable<DateTime> DataNascimento { get; set; }

        public virtual DateTime DataCadastro { get; set; }

        public virtual DateTime DataModificacao { get; set; }
    }
}
