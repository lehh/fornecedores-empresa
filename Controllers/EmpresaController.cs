using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.ViewModel;
using FornecedoresEmpresa.Models;

namespace FornecedoresEmpresa.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly ISession Sessao;

        public EmpresaController(ISession sessao)
        {
            this.Sessao = sessao;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaEmpresa = await new EmpresaDados(Sessao).ListarTodos();

            return View(listaEmpresa);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View(new EmpresaViewModelCadastro());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(EmpresaViewModelCadastro model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var empresa = new Empresa()
            {
                NomeFantasia = model.NomeFantasia,
                Cnpj = model.Cnpj,
                Uf = model.Uf,
                DataCadastro = DateTime.Now
            };

            try
            {
                new EmpresaDados(Sessao).Inserir(empresa);
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
                return View(model);
            }
            
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id) 
        {
            var empresa = await new EmpresaDados(Sessao).BuscarPorId(id);

            var model = new EmpresaViewModelDetalhes()
            {
                Id = empresa.Id,
                NomeFantasia = empresa.NomeFantasia,
                Uf = empresa.Uf,
                Cnpj = empresa.Cnpj,
                ListaFornecedor = empresa.Fornecedores.ToList(),
                DataCadastro = empresa.DataCadastro
            };

            return View(model);
        }
    }
}