using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Models;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class IdentityController : Controller
    {
        RepositoryTransacciones repo;
        public IdentityController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(String email, String password)
        {
            Usuario usr = this.repo.ValidarUsuario(email, password);

            if (usr == null)
            {
                ViewData["MENSAJE"] = "usuario o password incorrecto";
                return View();
            }
            else
            {

                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usr.IdUsuario.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, usr.ImagenUsuario));
                identity.AddClaim(new Claim(ClaimTypes.Role, usr.Email)); ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddMinutes(5)
                });
                HttpContext.Session.SetString("img", usr.ImagenUsuario);
                return RedirectToAction("Index", "Transacciones");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(String nombre, String nombreUsuario, String password, String email)
        {
            this.repo.InsertarUsuario(nombreUsuario, password, nombre, email);
            return RedirectToAction("Index", "Transacciones");
        }
    }
}
