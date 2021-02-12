using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Models;
using MoneyGo.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyGo.Filters;

namespace MoneyGo.Controllers
{
    public class LandingController : Controller
    {
        RepositoryTransacciones repo;

        public LandingController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }

        public object HttpSesion { get; private set; }

        [AuthorizeUsuarios]
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
                HttpContext.Session.SetInt32("user", user.IdUsuario);
                HttpContext.Session.SetString("nombre", user.Nombre);
                if (user.ImagenUsuario == null)
                {
                    HttpContext.Session.SetString("img", "vacio");
                }
                else
                {
                    HttpContext.Session.SetString("img", user.ImagenUsuario);
                }
                return RedirectToAction("Index", "Transacciones");
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
