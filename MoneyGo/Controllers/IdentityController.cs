using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Helpers;
using MoneyGo.Models;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class IdentityController : Controller
    {

        RepositoryTransacciones repo;
        MailService MailService;

        public IdentityController(RepositoryTransacciones repo, MailService MailService)
        {
            this.repo = repo;

            this.MailService = MailService;
        }
        #region Login y logout
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
                identity.AddClaim(new Claim(ClaimTypes.Name, usr.NombreUsuario));
                identity.AddClaim(new Claim(ClaimTypes.Email, usr.Email));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddMinutes(5)
                });


                HttpContext.Session.SetInt32("user", usr.IdUsuario);
                HttpContext.Session.SetString("img", usr.ImagenUsuario);
                return RedirectToAction("Index", "Transacciones");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(String nombre, String nombreUsuario, String password, String email)
        {
            this.repo.InsertarUsuario(nombreUsuario, password, nombre, email);
            return RedirectToAction("Index", "Landing");
        }

    }
}
