using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Filters;
using MoneyGo.Helpers;
using MoneyGo.Models;
using MoneyGo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class ManagementController : Controller
    {
        ServiceUsuario service;

        public ManagementController(ServiceUsuario service)
        {
            this.service = service;
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Index()
        {

            int id = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
         
            if (TempData["MSG"] != null)
            {
                ViewData["MSG"] = TempData["MSG"];
            }

            if (TempData["ERR"] != null)
            {
                ViewData["ERR"] = TempData["ERR"];
            }

            return View(await this.service.GetDataUsuario());

        }

        public IActionResult ChangePassword(String oldpassword, String newpassword, String passwordconfirm)
        {
            String email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            Usuario user = this.repo.ValidarUsuario(email, oldpassword);

            if (user != null && newpassword.Equals(passwordconfirm))
            {
                TempData["MSG"] = "Contraseña cambiada con éxito";
                this.repo.CambiarPassword(user, newpassword);

            }
            else if (user == null)
            {
                user = this.repo.getDataUsuario(int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                ViewData["ERR"] = "La contraseña antigua no es correcta";
                return RedirectToAction("Index", "Landing");
            }
            else
            {
                user = this.repo.getDataUsuario((int)HttpContext.Session.GetInt32("user"));
                TempData["ERR"] = "La contraseñas no coinciden";

            }

            return RedirectToAction("Index", "Management", user);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile imagen)
        {
            int id = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            Usuario user = this.repo.getDataUsuario(id);
              
            if (imagen != null)
            {

                String filename = imagen.FileName;
                String path = this.PathProvider.MapPath(filename, Folders.Images);

                if (filename != null)
                {
                    using (var Stream = new FileStream(path, FileMode.Create))
                    {
                        await imagen.CopyToAsync(Stream);
                    }
                    this.repo.UpdateImagen(id, filename);
                }
                ViewData["MSG"] = "Imagen cambiada con exito";


                HttpContext.Session.SetString("img", user.ImagenUsuario);
                return View(user);
            }
            else
            {
                ViewData["MSG"] = "Introduzca una imágen nueva para sustituir la imágen actual";

                return View(user);
            }

        }
    }
}
