using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.Models;
using FornecedoresEmpresa.Utils;
using NHibernate;

namespace FornecedoresEmpresa.Regras
{
    public class FornecedorRegras
    {
        private readonly FornecedorDados fornecedorDados;

        public FornecedorRegras(ISession sessao)
        {
            this.fornecedorDados = new FornecedorDados(sessao);
        }

        public async Task<ICollection<Fornecedor>> Filtrar(Fornecedor filtro)
        {
            return await fornecedorDados.BuscarTodos(filtro);
        }

        public async Task<bool> CadastrarAsync(Fornecedor fornecedor)
        {
            FornecedorValido(fornecedor);

            fornecedor = AdicionaTelefoneFornecedor(fornecedor);

            fornecedor.DataCadastro = DateTime.Today;

            await fornecedorDados.Inserir(fornecedor);

            return true;
        }

        public async Task<bool> AlterarAsync(Fornecedor fornecedor)
        {
            FornecedorValido(fornecedor);

            fornecedor = AdicionaTelefoneFornecedor(fornecedor);

            fornecedor.DataModificacao = DateTime.Now;

            await fornecedorDados.Alterar(fornecedor);

            return true;
        }

        /// <summary>
        /// Busca uma lista de fornecedores a partir de uma lista de ids e vincula a empresa. 
        /// </summary>
        public async Task<List<Fornecedor>> BuscaListaFornecedorAsync(List<int> listaIdFornecedor, Empresa empresa)
        {
            var listaFornecedor = new List<Fornecedor>();

            foreach (var fornecedorId in listaIdFornecedor)
            {
                var fornecedor = await fornecedorDados.BuscarPorId(fornecedorId);

                if (fornecedor == null)
                {
                    continue;
                }

                fornecedor.Empresa = empresa;

                listaFornecedor.Add(fornecedor);
            }

            return listaFornecedor;
        }

        public async Task<bool> AlterarVariosAsync(List<Fornecedor> listaFornecedor)
        {
            foreach (var fornecedor in listaFornecedor)
            {
                await fornecedorDados.Alterar(fornecedor);
            }

            return true;
        }

        public async Task<bool> RemoverAssociacaoComEmpresaAsync(int id)
        {
            var fornecedor = await fornecedorDados.BuscarPorId(id);

            if (fornecedor == null)
            {
                throw new UsuarioException("Fornecedor não encontrado");
            }

            fornecedor.Empresa = null;

            await fornecedorDados.Alterar(fornecedor);

            return true;
        }

        public async Task<bool> RemoverAssociadosComEmpresa(
           ISet<Fornecedor> listaFornecedorAtual,
           ISet<Fornecedor> listaFornecedorAtualizado
        )
        {
            var listaFornecedorParaRemover = listaFornecedorAtual
                        .Where(fornecedor => !listaFornecedorAtualizado.Select(f => f.Id).Contains(fornecedor.Id))
                        .ToList();

            foreach (var fornecedor in listaFornecedorParaRemover)
            {
                fornecedor.Empresa = null;
            }

            return await AlterarVariosAsync(listaFornecedorParaRemover);
        }

        private void FornecedorValido(Fornecedor fornecedor)
        {
            if (fornecedor.Cnpj == null && fornecedor.Cpf == null)
            {
                throw new UsuarioException("Preencha o CPF ou CNPJ");
            }

            if (fornecedor.Cnpj != null && fornecedor.Cpf != null)
            {
                throw new UsuarioException("Preencha apenas o CNPJ ou CPF");
            }

            if (fornecedor.Cpf != null && (!fornecedor.DataNascimento.HasValue || fornecedor.Rg == null))
            {
                throw new UsuarioException("Preencha a data de nascimento e o RG");
            }

            if (fornecedor.Telefones.Count <= 0)
            {
                throw new UsuarioException("Informe ao menos um telefone");
            }
        }

        private Fornecedor AdicionaTelefoneFornecedor(Fornecedor fornecedor)
        {
            foreach (var telefone in fornecedor.Telefones)
            {
                telefone.Fornecedor = fornecedor;
            }

            return fornecedor;
        }
    }
}
