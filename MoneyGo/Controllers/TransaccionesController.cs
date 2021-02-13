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
            int pos = 0;
            if (posicion == null)
            {
                pos = 1;
                
            }
            
            int registros = 0;
           

            var user = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Transacciones> transacciones = this.repo.GetTransaccionesPaginacion(pos, user, ref registros);
            ViewData["USUARIO"] = User.FindFirstValue(ClaimTypes.Name);   
           
            ViewData["NUMEROREGISTROS"] = registros;
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
