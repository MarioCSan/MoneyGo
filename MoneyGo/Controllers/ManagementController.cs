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
        PathProvider PathProvider;

        public ManagementController(ServiceUsuario service, PathProvider pathProvider)
        {
            this.service = service;
            this.PathProvider = pathProvider;
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Index()
        {
            int id = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            Usuario usuario = await this.service.GetDataUsuario(id);
         
            if (TempData["MSG"] != null)
            {
                ViewData["MSG"] = TempData["MSG"];
            }

            if (TempData["ERR"] != null)
            {
                ViewData["ERR"] = TempData["ERR"];
            }

            return View(usuario);

        }

        public async Task<IActionResult> ChangePassword(String oldpassword, String newpassword, String passwordconfirm)
        {
            String email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            Usuario user = await this.service.ValidarUsuario(email, oldpassword);

            if (user != null && newpassword.Equals(passwordconfirm))
            {
                TempData["MSG"] = "Contraseña cambiada con éxito";
               await this.service.ModificarPassword(newpassword);

            }
            else if(!newpassword.Equals(passwordconfirm))
            {
               
                TempData["ERR"] = "La contraseñas no coinciden";

            }
            else if (user == null)
            {

                ViewData["ERR"] = "La contraseña antigua no es correcta";
                return RedirectToAction("Index", "Landing");
            }

            return RedirectToAction("Index", "Management", user);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile imagen)
        {
            int id = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            Usuario user = await this.service.GetDataUsuario();
              
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
                    this.service.ModificarImagen(filename);
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
