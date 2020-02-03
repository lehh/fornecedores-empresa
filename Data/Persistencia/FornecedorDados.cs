using System;
using NHibernate;
using FornecedoresEmpresa.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace FornecedoresEmpresa.Data.Persistencia
{
    public class FornecedorDados : GenericoDados<Fornecedor, Int32>
    {
        public FornecedorDados(ISession sessao) : base(sessao) { }

        public async Task<List<Fornecedor>> BuscaFornecedoresDisponiveis()
        {
            ITransaction transacao = Sessao.BeginTransaction();

            try
            {
                var listaFornecedor = await Sessao.Query<Fornecedor>()
                    .Where(f => f.Empresa == null)
                    .ToListAsync();

                await transacao.CommitAsync();

                return listaFornecedor;
            }
            catch
            {
                await transacao.RollbackAsync();
                throw;
            }
        }

        public async Task<ICollection<Fornecedor>> BuscarTodos(Fornecedor filtro)
        {
            try
            {
                //var listaFornecedor = await Sessao.Query<Fornecedor>()
                //    .Where(f => f.Empresa == null)
                //    .ToListAsync();

                var query = from fornecedor in Sessao.Query<Fornecedor>()
                            where fornecedor.Cnpj.Contains(filtro.Cnpj)
                            || fornecedor.Cpf.Contains(filtro.Cpf)
                            || fornecedor.Nome.Contains(filtro.Nome)
                            || fornecedor.DataCadastro.Equals(filtro.DataCadastro)
                            || fornecedor.Rg.Equals(filtro.Rg)
                            select fornecedor;

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
