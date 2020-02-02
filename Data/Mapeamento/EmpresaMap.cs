using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Data.Mapeamento
{
    /// <summary>
    /// Mapeamento da classe Empresa
    /// </summary>
    public class EmpresaMap : ClassMap<Empresa>
    {
        public EmpresaMap()
        {
            Table("Empresa");

            Id(e => e.Id, "id").GeneratedBy.Identity();

            Map(e => e.NomeFantasia, "nome_fantasia").Length(150).Not.Nullable();
            Map(e => e.Uf, "uf").Length(2).Not.Nullable();
            Map(e => e.Cnpj, "cnpj").Length(14).Not.Nullable();
            Map(e => e.DataCadastro, "data_cadastro").Default("getdate()").Not.Nullable();
            Map(e => e.DataModificacao, "data_modificacao").Nullable();

            HasMany(e => e.Fornecedores).KeyColumn("id_empresa").Inverse();
        }
    }
}
