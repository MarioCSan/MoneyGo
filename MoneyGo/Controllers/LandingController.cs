using Microsoft.AspNetCore.Mvc;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class LandingController : Controller
    {
        RepositoryTransacciones repo;

        public LandingController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(String email, String password)
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(String email, String password)
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(String nombre, String nombreUsuario, String password, String email)
        {
            this.repo.InsertarUsuario(nombreUsuario, password, nombre, email);
            return RedirectToAction("Index");
        }
    }
}
