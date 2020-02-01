using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Data.Persistencia
{
    public class FornecedorDados : GenericoDados<Fornecedor, Int32>
    {
        public FornecedorDados(ISession sessao) : base(sessao) { }
    }
}
