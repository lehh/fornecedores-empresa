using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FornecedoresEmpresa.Data.Servico;
using NHibernate;
using NHibernate.Linq;

namespace FornecedoresEmpresa.Data.Persistencia
{
    public class GenericoDados<T, K> : IServicoBase<T, K>
        where T: class
        where K: struct
    {
        protected ISession Sessao;

        public GenericoDados(ISession sessao)
        {
            this.Sessao = sessao;
        }

        public async Task<bool> Alterar(T objeto)
        {
            ITransaction transacao = Sessao.BeginTransaction();

            try
            { 
                await Sessao.MergeAsync(objeto);
                await transacao.CommitAsync();

                return true;
            }
            catch
            {
                await transacao.RollbackAsync();
                throw;
            }
        }

        public async Task<T> BuscarPorId(K id)
        {
            return await Sessao.GetAsync<T>(id);
        }

        public async void Excluir(T objeto)
        {
            ITransaction transacao = Sessao.BeginTransaction();

            try
            {
                await Sessao.DeleteAsync(objeto);
                transacao.Commit();
            }
            catch
            {
                transacao.Rollback();
                throw;
            }
        }

        public async void Inserir(T objeto)
        {
            ITransaction transacao = Sessao.BeginTransaction();

            try
            {
                await Sessao.SaveAsync(objeto);
                transacao.Commit();
            }
            catch
            {
                transacao.Rollback();
                throw;
            }
           
        }

        public async Task<ICollection<T>> ListarTodos()
        {
            return await Sessao.Query<T>().ToListAsync();
        }
    }
}
