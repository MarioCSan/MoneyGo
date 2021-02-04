using Microsoft.AspNetCore.Http;
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
        public IActionResult Index()
        {
            int user = (int)HttpContext.Session.GetInt32("user");
            ViewData["USUARIO"] = HttpContext.Session.GetInt32("user");
            ViewData["NOMBRE"] = HttpContext.Session.GetString("nombre");
            List<Transacciones> transacciones = this.repo.GetTransacciones(user);

            return View(transacciones);
        }

        public IActionResult NuevaTransaccion(int id)
        {
            ViewData["user"] = id;
            return View();
        }
        [HttpPost]
        public IActionResult NuevaTransaccion(int IdUsuario, float cantidad, String tipoTransaccion, String Concepto)
        {
            String date = DateTime.Now.ToShortDateString();
            DateTime fecha = Convert.ToDateTime(date);

            this.repo.NuevaTransaccion(IdUsuario, cantidad, tipoTransaccion, Concepto, fecha);
            return RedirectToAction("Index", new { IdUsuario = IdUsuario});
        }

        public IActionResult Delete(int idtransaccion)
        {
            this.repo.EliminarTransaccion(idtransaccion);
            TempData["MENSAJE"] = "Transaccion eliminada correctamente";
            return RedirectToAction("Index");
        }
    }
}
