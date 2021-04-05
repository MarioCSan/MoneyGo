using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGoAPI.Models;
using MoneyGoAPI.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyGoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        RepositoryTransacciones repo;
        public TransaccionesController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public ActionResult<List<Transacciones>> GetTransaccionesUsuario(int id)
        {
            return this.repo.GetTransacciones(id);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public ActionResult<Transacciones> NuevaTransaccion(float cantidad, String tipoTransaccion, String Concepto)
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;
           
            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);

            DateTime date = DateTime.UtcNow;
            this.repo.NuevaTransaccion(usuario.IdUsuario, cantidad, tipoTransaccion, Concepto, date);
            return RedirectToAction("GetTransaccionesUsuario");
        }
    }
}
