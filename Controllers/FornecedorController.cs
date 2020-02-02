using System;
using System.Threading.Tasks;
using System.Collections.Generic;
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
            var model = new FornecedorViewModelCadastro();
            model.ListaTelefoneFornecedor.Add(new TelefoneFornecedorPartialViewModel());

            return View(model);
        }

        [HttpPost]
        public IActionResult Cadastrar(FornecedorViewModelCadastro model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var setTelefoneFornecedor = new TelefoneFornecedorRegras(Sessao)
                                        .CriaTelefoneFornecedores(model.ListaTelefoneFornecedor);

            var fornecedor = new Fornecedor()
            {
                Nome = model.Nome,
                Cpf = model.Cpf,
                Cnpj = model.Cnpj,
                Rg = model.Rg,
                DataNascimento = model.DataNascimento,
                Telefones = setTelefoneFornecedor
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

        public ViewResult AdicionarTelefone(int indice, string nomeLista, string divId)
        {
            var model = new TelefoneFornecedorPartialViewModel()
            {
                DivId = divId,
                NomeLista = nomeLista,
                Indice = indice
            };

            return View("_TelefoneFornecedor", model);
        }
    }
}