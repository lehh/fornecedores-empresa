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
using System.Linq;

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

            var model = new FornecedorViewModelIndex();

            model.ListaFornecedor = listaFornecedor;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Filtrar(FornecedorViewModelIndex model)
        {
            var listaFornecedor = await new FornecedorDados(Sessao).BuscarTodos(model.Fornecedor);

            model.ListaFornecedor = listaFornecedor;

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var model = new FornecedorViewModelCadastro();
            model.ListaTelefoneFornecedor.Add(new TelefoneFornecedorPartialViewModel());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(FornecedorViewModelCadastro model)
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
                await new FornecedorRegras(Sessao).CadastrarAsync(fornecedor);

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

        [HttpGet]
        public async Task<IActionResult> Alterar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var fornecedor = await new FornecedorDados(Sessao).BuscarPorId((int)id);

            if (fornecedor == null)
            {
                TempData["Mensagem"] = "Fornecedor não encontrado";
                return RedirectToAction("Index");
            }

            var listaModelTelefone = new List<TelefoneFornecedorPartialViewModel>();

            foreach (var telefone in fornecedor.Telefones)
            {
                listaModelTelefone.Add(new TelefoneFornecedorPartialViewModel()
                {
                    Numero = telefone.Numero,
                    Id = telefone.Id
                });
            }

            var model = new FornecedorViewModelAlterar()
            {
                Id = fornecedor.Id,
                Cnpj = fornecedor.Cnpj,
                Cpf = fornecedor.Cpf,
                DataNascimento = fornecedor.DataNascimento,
                Nome = fornecedor.Nome,
                Rg = fornecedor.Rg,
                ListaTelefoneFornecedor = listaModelTelefone
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(FornecedorViewModelAlterar model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var fornecedor = await new FornecedorDados(Sessao).BuscarPorId(model.Id);

                if (fornecedor == null)
                {
                    TempData["Mensagem"] = "Fornecedor não encontrado";
                    return RedirectToAction("Index");
                }

                var telefoneFornecedorRegras = new TelefoneFornecedorRegras(Sessao);

                var setTelefoneFornecedor = telefoneFornecedorRegras.CriaTelefoneFornecedores(model.ListaTelefoneFornecedor);
                await telefoneFornecedorRegras.ExcluiTelefonesFornecedorAsync(fornecedor.Telefones, setTelefoneFornecedor);

                fornecedor.Nome = model.Nome;
                fornecedor.Rg = model.Rg;
                fornecedor.Cpf = model.Cpf;
                fornecedor.Cnpj = model.Cnpj;
                fornecedor.DataNascimento = model.DataNascimento;
                fornecedor.Telefones = setTelefoneFornecedor;

                await new FornecedorRegras(Sessao).AlterarAsync(fornecedor);

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

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var fornecedorDados = new FornecedorDados(Sessao);
            var fornecedor = await fornecedorDados.BuscarPorId((int)id);

            if (fornecedor == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                await fornecedorDados.Excluir(fornecedor);
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoverAssociacao(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Empresa");
            }

            try
            {
                await new FornecedorRegras(Sessao).RemoverAssociacaoComEmpresaAsync((int)id);
            }
            catch (UsuarioException uex)
            {
                TempData["Mensagem"] = uex.Message;
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
            }

            var url = HttpContext.Request.Headers["Referer"];

            return Redirect(url);
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