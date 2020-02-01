using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FornecedoresEmpresa.Data.Persistencia;
using NHibernate;

namespace FornecedoresEmpresa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession sessao;
        public HomeController(ISession sessao)
        {
            this.sessao = sessao;
        }

        public async Task<IActionResult> Index()
        {
            var fornecedor = new FornecedorDados(sessao);

            var teste = await fornecedor.ListarTodos();

            return View(teste);
        }
    }
}