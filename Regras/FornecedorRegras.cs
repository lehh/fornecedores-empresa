using System;
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

        public void Cadastrar(Fornecedor fornecedor)
        {
            FornecedorValido(fornecedor);

            fornecedor = AdicionaTelefoneFornecedor(fornecedor);

            fornecedorDados.Inserir(fornecedor);
        }

        public void Alterar(Fornecedor fornecedor)
        {
            FornecedorValido(fornecedor);

            fornecedor = AdicionaTelefoneFornecedor(fornecedor);

            fornecedorDados.Alterar(fornecedor);
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
