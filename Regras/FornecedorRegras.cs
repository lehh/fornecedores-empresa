using System;
using System.Threading.Tasks;
using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.Models;
using FornecedoresEmpresa.Utils;

namespace FornecedoresEmpresa.Regras
{
    public class FornecedorRegras
    {
        private readonly FornecedorDados fornecedorDados;

        public FornecedorRegras(FornecedorDados fornecedorDados)
        {
            this.fornecedorDados = fornecedorDados;
        }

        public async Task<bool> CadastrarAsync(Fornecedor fornecedor)
        {
            FornecedorValido(fornecedor);

            fornecedor = await AdicionaTelefoneFornecedorAsync(fornecedor);

            fornecedorDados.Inserir(fornecedor);

            return true;
        }

        public async Task<bool> AlterarAsync(Fornecedor fornecedor)
        {
            FornecedorValido(fornecedor);

            fornecedor = await AdicionaTelefoneFornecedorAsync(fornecedor);

            await fornecedorDados.Alterar(fornecedor);

            return true;
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

        private async Task<Fornecedor> AdicionaTelefoneFornecedorAsync(Fornecedor fornecedor)
        {
            foreach (var telefone in fornecedor.Telefones)
            {
                telefone.Fornecedor = fornecedor;
            }

            return fornecedor;
        }
    }
}
