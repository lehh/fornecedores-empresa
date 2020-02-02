using FluentNHibernate.Mapping;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Data.Mapeamento
{
    public class TelefoneFornecedorMap : ClassMap<TelefoneFornecedor>
    {
        public TelefoneFornecedorMap()
        {
            Table("Telefone_Fornecedor");

            Id(t => t.Id, "id").GeneratedBy.Identity();

            Map(t => t.Numero, "numero").Length(15).Not.Nullable();

            References(t => t.Fornecedor).Column("id_fornecedor").Not.Nullable();
        }
    }
}
