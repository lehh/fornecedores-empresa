using System;
using System.Collections.Generic;
using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.Models;
using FornecedoresEmpresa.ViewModel;
using FornecedoresEmpresa.Utils;
using NHibernate;
using System.Linq;
using System.Threading.Tasks;

namespace FornecedoresEmpresa.Regras
{
    public class TelefoneFornecedorRegras
    {
        private readonly TelefoneFornecedorDados telefoneFornecedorDados;

        public TelefoneFornecedorRegras(ISession sessao)
        {
            this.telefoneFornecedorDados = new TelefoneFornecedorDados(sessao);
        }

        public HashSet<TelefoneFornecedor> CriaTelefoneFornecedores(IEnumerable<TelefoneFornecedorViewModel> listaModel)
        {
            var setTelefoneFornecedor = new HashSet<TelefoneFornecedor>();

            foreach (var telefone in listaModel)
            {
                if (!ValidaNumeroTelefone(telefone.Numero))
                {
                    continue;
                }

                setTelefoneFornecedor.Add(new TelefoneFornecedor()
                {
                    Numero = telefone.Numero,
                    Id = telefone.Id
                });
            }

            return setTelefoneFornecedor;
        }

        public async Task<int> ExcluiTelefonesFornecedorAsync(
            ISet<TelefoneFornecedor> listaTelefoneAtual,
            ISet<TelefoneFornecedor> listaTelefoneAtualizado
        )
        {
            var listaIdParaExcluir = listaTelefoneAtual
                        .Where(telefone => !listaTelefoneAtualizado.Select(t => t.Id).Contains(telefone.Id))
                        .Select(telefone => telefone.Id)
                        .ToList();

            if (listaIdParaExcluir.Count > 0)
            {
               return await telefoneFornecedorDados.ExcluirVariosAsync(listaIdParaExcluir);
            }

            return 0;
        }

        private bool ValidaNumeroTelefone(string numeroTelefone)
        {
            if (numeroTelefone == null)
            {
                return false;
            }

            return true;
        }
    }
}
