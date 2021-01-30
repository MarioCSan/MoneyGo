using Microsoft.AspNetCore.Mvc;
using MoneyGo.Models;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class TransaccionesController : Controller
    {
        RepositoryTransacciones repo;
        public TransaccionesController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }
        public IActionResult Index(int IdUsuario, String nombre)
        {
            ViewData["USUARIO"] = nombre;
            List<Transacciones> transacciones = this.repo.GetTransacciones(IdUsuario);

            return View(transacciones);
        }

        public IActionResult NuevaTransaccion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NuevaTransaccion(int IdUsuario, float cantidad, String tipoTransaccion)
        {
            String date = DateTime.Now.ToShortDateString();
            DateTime fecha = Convert.ToDateTime(date);

            this.repo.NuevaTransaccion(IdUsuario, cantidad, tipoTransaccion, fecha);
            return View();
        }
    }
}
