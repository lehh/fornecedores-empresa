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

        public bool Cadastrar(Fornecedor fornecedor)
        {
            FornecedorValido(fornecedor);

            fornecedorDados.Inserir(fornecedor);

            return true;
        }

        private void FornecedorValido(Fornecedor fornecedor)
        {
            if (fornecedor.Cnpj != null && fornecedor.Cpf != null)
            {
                throw new UsuarioException("Preencha apenas o CNPJ ou CPF");
            }

            if (fornecedor.Cpf != null && (!fornecedor.DataNascimento.HasValue || fornecedor.Rg == null))
            {
                throw new UsuarioException("Preencha a data de nascimento e o RG");
            }
        }
    }
}
