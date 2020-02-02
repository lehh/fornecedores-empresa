using System;
using NHibernate;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Data.Persistencia
{
    public class TelefoneFornecedorDados : GenericoDados<TelefoneFornecedor, Int32>
    {
        public TelefoneFornecedorDados(ISession sessao) : base(sessao) { }
    }
}
