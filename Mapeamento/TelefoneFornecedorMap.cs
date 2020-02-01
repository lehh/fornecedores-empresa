using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Mapeamento
{
    public class TelefoneFornecedorMap : ClassMap<TelefoneFornecedor>
    {
        public TelefoneFornecedorMap()
        {
            Table("Telefone_Fornecedor");

            Id(t => t.Id, "id").GeneratedBy.Identity();

            Map(t => t.Numero).Not.Nullable();

            References(t => t.Fornecedor).Column("id_fornecedor").Not.Nullable();
        }
    }
}
