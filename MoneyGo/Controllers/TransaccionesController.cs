using Microsoft.AspNetCore.Mvc;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class TransaccionesController : Controller
    {
        RepositoryTransacciones repo;
        public TransaccionesController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }
        public IActionResult Index(String nombre)
        {
            ViewData["USUARIO"] = nombre;
            return View();
        }
    }
}
