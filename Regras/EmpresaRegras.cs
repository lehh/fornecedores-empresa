using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FornecedoresEmpresa.Utils;

namespace FornecedoresEmpresa.Regras
{
    public class EmpresaRegras
    {
        private readonly EmpresaDados empresaDados;

        public EmpresaRegras(ISession sessao)
        {
            this.empresaDados = new EmpresaDados(sessao);
        }

        public async Task<bool> CadastrarAsync(Empresa empresa)
        {
            ValidaFornecedores(empresa.Fornecedores, empresa);

            empresa.DataCadastro = DateTime.Today;

            await empresaDados.Inserir(empresa);

            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var empresa = await empresaDados.BuscarPorId((int)id);

            ValidaExclusao(empresa);

            await empresaDados.Excluir(empresa);

            return true;
        }

        private void ValidaExclusao(Empresa empresa)
        {
            if (empresa == null)
            {
                throw new UsuarioException("Empresa não encontrada");
            }

            if (empresa.Fornecedores.Count > 0)
            {
                throw new UsuarioException("Não é possível excluir empresas com fornecedores vinculados. Por favor, desassocie-os");
            }
        }

        private void ValidaFornecedores(ISet<Fornecedor> listaFornecedor, Empresa empresa)
        {
            foreach (var fornecedor in listaFornecedor)
            {
                if (fornecedor.Cnpj == null)
                {
                    ValidaPessoaFisica(fornecedor, empresa);
                }
            }
        }

        private void ValidaPessoaFisica(Fornecedor fornecedor, Empresa empresa)
        {
            var ufEmpresa = empresa.Uf;

            var idadeFornecedor = Utilitarios.CalculaIdade(fornecedor.DataNascimento.Value, DateTime.Today);

            if (ufEmpresa == "PR" && idadeFornecedor < 18)
            {
                throw new UsuarioException("Não é possível cadastrar o fornecedor " + fornecedor.Nome + " para o Paraná pois ele é menor de idade");
            }
        }
    }
}
