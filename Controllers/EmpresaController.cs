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
        public IActionResult Cadastrar()
        {
            return View(new EmpresaViewModelCadastro());
        }

        [HttpPost]
        public IActionResult Cadastrar(EmpresaViewModelCadastro model)
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

            return RedirectToAction("Index");
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

            var model = new EmpresaViewModelAlterar()
            {
                Cnpj = empresa.Cnpj,
                NomeFantasia = empresa.NomeFantasia,
                Uf = empresa.Uf
            };

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

            empresa.Id = model.Id;
            empresa.NomeFantasia = model.NomeFantasia;
            empresa.Cnpj = model.Cnpj;
            empresa.Uf = model.Uf;
            empresa.DataModificacao = DateTime.Now;

            try
            {
                empresaDados.Alterar(empresa);
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var empresaDados = new EmpresaDados(Sessao);
            var empresa = await empresaDados.BuscarPorId((int)id);

            if (empresa == null) 
            {
                return RedirectToAction("Index");
            }

            try
            {
                empresaDados.Excluir(empresa);
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}