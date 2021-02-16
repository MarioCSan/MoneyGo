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
        public IActionResult Index(int? posicion)
        {

            if (posicion == null)
            {
                posicion = 1;
                
            }

            int registros = 0;
           

            int user = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Transacciones> transacciones = this.repo.GetTransaccionesPaginacion(posicion.Value, user, ref registros);

            var json = HelperToolkit.SerializeJsonObject(transacciones);

            ViewData["USUARIO"] = User.FindFirstValue(ClaimTypes.Name);
            ViewData["ID"] = user;
            ViewData["NUMEROREGISTROS"] = registros;
            ViewData["json"] = json;
            return View(transacciones);
        }

        [HttpPost]
        public IActionResult NuevaTransaccion(Double cantidad, String tipoTransaccion, String Concepto)
        {
            String date = DateTime.Now.ToShortTimeString();
            DateTime fecha = Convert.ToDateTime(date);
            int IdUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            this.repo.NuevaTransaccion(IdUsuario, cantidad, tipoTransaccion, Concepto, fecha);
            ViewData["MSG"] = "Transacción creada";
            return RedirectToAction("Index", "Transacciones");
        }

        public IActionResult Delete(int idtransaccion)
        {
            this.repo.EliminarTransaccion(idtransaccion);
            TempData["MENSAJE"] = "Transaccion eliminada correctamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ModificarTransaccion(int idtransaccion, Double cantidad, string tipoTransaccion, string concepto)
        {
            this.repo.ModificarTransaccion(idtransaccion, cantidad, tipoTransaccion, concepto);
            TempData["MENSAJE"] = "Transaccion Modificada correctamente";
            return RedirectToAction("Index");
        }

        public IActionResult OrdenarIngresoAsc()
        {
            int IdUsuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).ToString());

            List<Transacciones> transacciones = this.repo.GetTransaccionesAsc(IdUsuario, "Ingresos");

            return View(transacciones);

        }
    }
}
