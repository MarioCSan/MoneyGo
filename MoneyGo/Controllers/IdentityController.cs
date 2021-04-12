using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Helpers;
using MoneyGo.Models;
using MoneyGo.Services;
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

        ServiceUsuario ApiService;
        MailService MailService;

        public IdentityController(ServiceUsuario service,MailService MailService)
        {
            this.ApiService = service;

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
            String token = await this.ApiService.GetToken(email, password);

            if (token == null)
            {
                ViewData["MENSAJE"] = "usuario o password incorrecto";
                return View();
            }
            else
            {
                Usuario user = await this.ApiService.GetDataUsuario(token);

                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.NombreUsuario));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.UserData, user.ImagenUsuario));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddMinutes(5)
                });

                if (user.ImagenUsuario == null)
                {
                    user.ImagenUsuario = "UserLogo.svg";
                }
                HttpContext.Session.SetString("img", user.ImagenUsuario);
                return RedirectToAction("Index", "Transacciones");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Landing");
        }

        #endregion

        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(String nombre, String nombreUsuario, String password, String email)
        //{
        //    this.repo.InsertarUsuario(nombreUsuario, password, nombre, email);
        //    return RedirectToAction("Index", "Landing");
        //}
      
    }
}
