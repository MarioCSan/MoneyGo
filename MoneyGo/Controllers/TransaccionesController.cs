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
           

            var user = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Transacciones> transacciones = this.repo.GetTransaccionesPaginacion(posicion.Value, user, ref registros);
            ViewData["USUARIO"] = User.FindFirstValue(ClaimTypes.Name);
            ViewData["ID"] = user;
            ViewData["NUMEROREGISTROS"] = registros;
            return View(transacciones);
        }

        [HttpPost]
        public IActionResult NuevaTransaccion(float cantidad, String tipoTransaccion, String Concepto)
        {
            String date = DateTime.Now.ToShortDateString();
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
    }
}
