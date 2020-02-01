using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Mapeamento
{
    public class FornecedorMap : SubclassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");

            KeyColumn("id_pessoa");

            References(f => f.Empresa).Column("id_empresa").Nullable();

            HasMany(f => f.Telefones).KeyColumn("id_fornecedor").Inverse().Cascade.All();
        }
    }
}
