using Microsoft.AspNetCore.Mvc;
using MoneyGo.Models;
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
      
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(String email, String password)
        {
            Usuario user = this.repo.ValidarUsuario(email, password);
            if (user == null)
            {
                ViewData["MENSAJE"] = "usuario/password no válidos";
                return View();
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales correctas, " + user.Nombre;
                return RedirectToAction("Index", "Transacciones", new { IdUsuario = user.IdUsuario, nombre = user.NombreUsuario});
            }
           
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(String nombre, String nombreUsuario, String password, String email)
        {
            this.repo.InsertarUsuario(nombreUsuario, password, nombre, email);
            ViewData["MENSAJE"] = "Revise la bandeja de entrada de su email";
            return View();
        }
    }
}
