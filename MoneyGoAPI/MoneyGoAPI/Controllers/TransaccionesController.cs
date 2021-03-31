using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGoAPI.Models;
using MoneyGoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        
    }
}
