using System;
using FornecedoresEmpresa.Models;
using NHibernate;

namespace FornecedoresEmpresa.Data.Persistencia
{
    public class EmpresaDados : GenericoDados<Empresa, Int32>
    {
        public EmpresaDados(ISession sessao) : base(sessao) {}
    }
}
