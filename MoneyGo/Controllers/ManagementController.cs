using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Models;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class ManagementController : Controller
    {
        RepositoryTransacciones repo;

        public ManagementController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            int id = (int)HttpContext.Session.GetInt32("id");
            Usuario user = this.repo.getDataUsuario(id);
            
            return View(user);
        }
    }
}
