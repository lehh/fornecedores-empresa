using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.ViewModel;
using FornecedoresEmpresa.Models;
using FornecedoresEmpresa.Regras;
using FornecedoresEmpresa.Utils;

namespace FornecedoresEmpresa.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly ISession Sessao;

        public FornecedorController(ISession sessao)
        {
            this.Sessao = sessao;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaFornecedor = await new FornecedorDados(Sessao).ListarTodos();

            return View(listaFornecedor);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View(new FornecedorViewModelCadastro());
        }

        [HttpPost]
        public IActionResult Cadastrar(FornecedorViewModelCadastro model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var fornecedor = new Fornecedor()
            {
                Nome = model.Nome,
                Cpf = model.Cpf,
                Cnpj = model.Cnpj,
                DataNascimento = model.DataNascimento,
                DataCadastro = DateTime.Now
            };

            try
            {
                var fornecedorDados = new FornecedorDados(Sessao);
                new FornecedorRegras(fornecedorDados).Cadastrar(fornecedor);
                
                return RedirectToAction("Index");
            }
            catch (UsuarioException uex)
            {
                TempData["Mensagem"] = uex.Message;
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro Interno: " + ex.Message;
            }

            return View(model);
        }
    }
}