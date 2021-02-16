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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MoneyGo.Helpers;
using System.Security.Claims;

namespace MoneyGo.Controllers
{
    public class LandingController : Controller
    {
       
        RepositoryTransacciones repo;
        MailService mailService;
        public LandingController(RepositoryTransacciones repo, MailService mailService)
        {
            this.repo = repo;
            this.mailService = mailService;
        }

        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(String nombre, String nombreUsuario, String password, String RepetirPassword, String email)
        {
            if(password == RepetirPassword) {
                bool valido = this.repo.BuscarEmail(email);

                if (valido)
                {
                    this.repo.InsertarUsuario(nombreUsuario, password, nombre, email);
                    ViewData["MENSAJE"] = "Revise la bandeja de entrada de su email.";
                }
                else
                {
                    ViewData["Error"] = "El email ya esta en uso.";
                }
                
            } else
            {
                ViewData["Error"] = "Las contraseñas no son iguales.";
            }
            
            return View();
        }



        public IActionResult RecuperarPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarPassword(String email)
        {
            
            Usuario usuario = this.repo.GetUsuarioEmail(email);
            if (usuario != null)
            {
                // Token => cadena aleatorea de 16 caracteres numerocos??

                var token = this.repo.GenerarToken(); // await Usuario.GeneratePasswordResetTokenAsync(usuario);
                var link = Url.Action("ResetPassword", "Landing", new { token, email = email }, Request.Scheme);
                this.mailService.SendEmailRecuperacion(email, link); 
                ViewData["MSG"] = "Se ha enviado un email de recuperación. Revise su correo.";

            }
            else
            {
                ViewData["MSG"] = "No existe ninguna cuenta asociada al email introducido.";

            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            Usuario usuario = this.repo.GetUsuarioEmail(email);
            ViewData["token"] = token;
            return View(usuario);

        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ResetPassword(String email, String password, String passwordConfirm)
        {
            if (password.Equals(passwordConfirm))
            {
                Usuario usuario = this.repo.GetUsuarioEmail(email);
                this.repo.CambiarPassword(usuario, password);

                return RedirectToAction("Index", "Landing");

            }
            else
            {
                ViewData["ERROR"] = "Las contraseñas no son iguales";
                return View();
            }


        }

        public IActionResult PoliticaCookies()
        {
            return View();
        }
    }
}
