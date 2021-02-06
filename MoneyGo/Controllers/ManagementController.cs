using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Helpers;
using MoneyGo.Models;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class ManagementController : Controller
    {
        RepositoryTransacciones repo;
        PathProvider PathProvider;

        public ManagementController(RepositoryTransacciones repo, PathProvider PathProvider)
        {
            this.repo = repo;
            this.PathProvider = PathProvider;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("Index", "Landing");
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("user");
                Usuario user = this.repo.getDataUsuario(id);

                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile imagen)
        {
            Usuario user = this.repo.getDataUsuario((int)HttpContext.Session.GetInt32("user"));
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
                    this.repo.UpdateImagen((int)HttpContext.Session.GetInt32("user"), filename);
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
