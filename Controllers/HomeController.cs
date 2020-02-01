using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using FornecedoresEmpresa;

namespace FornecedoresEmpresa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession sessao;
        public HomeController(ISession sessao)
        {
            this.sessao = sessao;
        }

        public IActionResult Index()
        {
            var teste = sessao.Query<Models.Fornecedor>().ToList();
            return View(teste);
        }
    }
}