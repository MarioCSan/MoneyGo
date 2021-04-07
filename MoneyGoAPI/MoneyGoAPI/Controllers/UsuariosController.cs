using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Helpers;
using MoneyGoAPI.Models;
using MoneyGoAPI.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyGoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        RepositoryTransacciones repo;
        PathProvider PathProvider;
        public UsuariosController(RepositoryTransacciones repo, PathProvider path)
        {
            this.repo = repo;
            this.PathProvider = path;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<Usuarios> GetDataUsuario()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;

            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);
            return this.repo.getDataUsuario(usuario.IdUsuario);
        }

        [HttpPut]
        [Route("[action]")]
        public ActionResult<Usuarios> ModificarPassword(Usuarios usuario, String password)
        {

            this.repo.CambiarPassword(usuario, password);
            return RedirectToAction("GetTransaccionesUsuario");
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult<Usuarios>> ModificarImagenAsync(IFormFile imagen)
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);

            String filename = imagen.FileName;
            String path = this.PathProvider.MapPath(filename, Folders.Images);
            if (filename != null)
            {
                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    await imagen.CopyToAsync(Stream);
                }
                this.repo.UpdateImagen(usuario.IdUsuario, filename);
            }
         
            return RedirectToAction("GetTransaccionesUsuario");
        }

        [HttpDelete]
        [Route("[action]")]
        public ActionResult<Usuarios> EliminarCuenta()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;

            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);

            this.repo.EliminarCuenta(usuario.IdUsuario);
            return RedirectToAction("Auth", "Index");
        }
    }
}
