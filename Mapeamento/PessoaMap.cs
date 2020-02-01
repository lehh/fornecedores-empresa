using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Mapeamento
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Table("Pessoa");

            Id(p => p.Id, "id").GeneratedBy.Identity();

            Map(p => p.Nome, "nome").Not.Nullable();
            Map(p => p.Cnpj, "cnpj").Length(14).Nullable();
            Map(p => p.Cpf, "cpf").Length(11).Nullable();
            Map(p => p.Rg, "rg").Length(14).Nullable();
            Map(p => p.DataNascimento, "data_nascimento").Nullable();
            Map(e => e.DataCadastro, "data_cadastro").Not.Nullable();
            Map(e => e.DataModificacao, "data_modificacao").Nullable();
        }
    }
}
