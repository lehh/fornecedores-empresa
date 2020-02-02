using System;
using System.Collections.Generic;
using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.Models;
using FornecedoresEmpresa.ViewModel;
using FornecedoresEmpresa.Utils;
using NHibernate;

namespace FornecedoresEmpresa.Regras
{
    public class TelefoneFornecedorRegras
    {
        private readonly TelefoneFornecedorDados telefoneFornecedorDados;

        public TelefoneFornecedorRegras(TelefoneFornecedorDados telefoneFornecedorDados)
        {
            this.telefoneFornecedorDados = telefoneFornecedorDados;
        }

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
