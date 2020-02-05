using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using FornecedoresEmpresa.Data.Persistencia;
using FornecedoresEmpresa.ViewModel;
using FornecedoresEmpresa.Models;
using FornecedoresEmpresa.Regras;
using FornecedoresEmpresa.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var empresa = await new EmpresaDados(Sessao).BuscarPorId((int)id);

            if (empresa == null)
            {
                return RedirectToAction("Index");
            }

            var model = new EmpresaViewModelDetalhes()
            {
                NomeFantasia = empresa.NomeFantasia,
                Uf = empresa.Uf,
                Cnpj = empresa.Cnpj,
                ListaFornecedor = empresa.Fornecedores.ToList(),
                DataCadastro = empresa.DataCadastro
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar()
        {
            var collectionFornecedor = await new FornecedorDados(Sessao).BuscaFornecedoresDisponiveis();

            var model = new EmpresaViewModelCadastro();

            foreach (var fornecedor in collectionFornecedor)
            {
                model.SelectListaFornecedor.Add(new SelectListItem()
                {
                    Value = fornecedor.Id.ToString(),
                    Text = fornecedor.Nome
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(EmpresaViewModelCadastro model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var empresa = new Empresa()
                {
                    NomeFantasia = model.NomeFantasia,
                    Cnpj = model.Cnpj,
                    Uf = model.Uf,
                    DataCadastro = DateTime.Now
                };

                var listaFornecedores = await new FornecedorDados(Sessao).BuscaFornecedoresDisponiveis();

                foreach (var fornecedor in listaFornecedores)
                {
                    model.SelectListaFornecedor.Add(new SelectListItem(fornecedor.Nome, fornecedor.Id.ToString()));
                }

                var fornecedorRegras = new FornecedorRegras(Sessao);
                var listaFornecedor = await fornecedorRegras.BuscaListaFornecedorAsync(model.ListaIdFornecedor, empresa);

                empresa.Fornecedores = new HashSet<Fornecedor>(listaFornecedor);

                await new EmpresaRegras(Sessao).CadastrarAsync(empresa);

                TempData["Mensagem"] = "Empresa cadastrada com sucesso!";

                return RedirectToAction("Index");
            }
            catch (UsuarioException uex)
            {
                TempData["Mensagem"] = uex.Message;
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
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

            var empresa = await new EmpresaDados(Sessao).BuscarPorId((int)id);

            if (empresa == null)
            {
                return RedirectToAction("Index");
            }

            var model = new EmpresaViewModelAlterar();

            var listaFornecedores = await new FornecedorDados(Sessao).BuscaFornecedoresDisponiveis();

            foreach (var fornecedor in listaFornecedores)
            {
                model.SelectListaFornecedor.Add(new SelectListItem(fornecedor.Nome, fornecedor.Id.ToString()));
            }

            foreach (var fornecedor in empresa.Fornecedores)
            {
                model.SelectListaFornecedor.Add(new SelectListItem(fornecedor.Nome, fornecedor.Id.ToString(), true));
            }

            model.Cnpj = empresa.Cnpj;
            model.NomeFantasia = empresa.NomeFantasia;
            model.Uf = empresa.Uf;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(EmpresaViewModelAlterar model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var empresaDados = new EmpresaDados(Sessao);
            var empresa = await empresaDados.BuscarPorId(model.Id);

            if (empresa == null)
            {
                TempData["Mensagem"] = "Empresa não encontrada";
                return RedirectToAction("Index");
            }

            var listaFornecedores = await new FornecedorDados(Sessao).BuscaFornecedoresDisponiveis();

            foreach (var fornecedor in listaFornecedores)
            {
                model.SelectListaFornecedor.Add(new SelectListItem(fornecedor.Nome, fornecedor.Id.ToString()));
            }

            foreach (var fornecedor in empresa.Fornecedores)
            {
                model.SelectListaFornecedor.Add(new SelectListItem(fornecedor.Nome, fornecedor.Id.ToString(), true));
            }

            empresa.NomeFantasia = model.NomeFantasia;
            empresa.Cnpj = model.Cnpj;
            empresa.Uf = model.Uf;
            empresa.DataModificacao = DateTime.Now;

            var fornecedorRegras = new FornecedorRegras(Sessao);
            var listaFornecedor = await fornecedorRegras.BuscaListaFornecedorAsync(model.ListaIdFornecedor, empresa);

            var setFornecedor = new HashSet<Fornecedor>(listaFornecedor);

            await fornecedorRegras.RemoverAssociadosComEmpresa(empresa.Fornecedores, setFornecedor);

            empresa.Fornecedores = setFornecedor;

            try
            {
                await new EmpresaRegras(Sessao).AlterarAsync(empresa);

                TempData["Mensagem"] = "Empresa salva com sucesso!";

                return RedirectToAction("Index");
            }
            catch (UsuarioException uex)
            {
                TempData["Mensagem"] = uex.Message;
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
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

            try
            {
                var empresaRegras = new EmpresaRegras(Sessao);
                await empresaRegras.ExcluirAsync((int)id);

                TempData["Mensagem"] = "Empresa excluída com sucesso!";
            }
            catch (UsuarioException uex)
            {
                TempData["Mensagem"] = uex.Message;
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}