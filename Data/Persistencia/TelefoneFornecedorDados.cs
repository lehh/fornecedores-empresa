using System;
using NHibernate;
using FornecedoresEmpresa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NHibernate.Linq;
using System.Threading;

namespace FornecedoresEmpresa.Data.Persistencia
{
    public class TelefoneFornecedorDados : GenericoDados<TelefoneFornecedor, Int32>
    {
        public TelefoneFornecedorDados(ISession sessao) : base(sessao) { }

        public async Task<int> ExcluirVariosAsync(List<int> listaId)
        {
            ITransaction transacao = Sessao.BeginTransaction();
            var cancellationToken = new CancellationToken();

            try
            {
                var resultado = await Sessao.Query<TelefoneFornecedor>()
                    .Where(t => listaId.Contains(t.Id))
                    .DeleteAsync(cancellationToken);

                await transacao.CommitAsync(cancellationToken);

                return resultado;
            }
            catch
            {
                await transacao.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
