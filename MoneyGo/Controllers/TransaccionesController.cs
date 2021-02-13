using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Filters;
using MoneyGo.Helpers;
using MoneyGo.Models;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [AuthorizeUsuarios]
        public IActionResult Index()
        {
            var user = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            ViewData["USUARIO"] = User.FindFirstValue(ClaimTypes.Name);   
            List<Transacciones> transacciones = this.repo.GetTransacciones(user);

            return View(transacciones);
        }
        //prueba del sanitizer
        [HttpPost]
        public IActionResult Index(String filename) {

            String sanitize = HelperToolkit.Normalize(filename);
            ViewData["CADENA"] = sanitize;
            return View();
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
            ViewData["MSG"] = "Transacción creada";
            return RedirectToAction("Index", "Transacciones", new { IdUsuario = IdUsuario});
        }

        public IActionResult Delete(int idtransaccion)
        {
            this.repo.EliminarTransaccion(idtransaccion);
            TempData["MENSAJE"] = "Transaccion eliminada correctamente";
            return RedirectToAction("Index");
        }
    }
}
