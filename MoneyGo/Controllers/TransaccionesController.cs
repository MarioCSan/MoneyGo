using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Filters;
using MoneyGo.Helpers;
using MoneyGo.Models;
using MoneyGo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyGo.Controllers
{
    public class TransaccionesController : Controller
    {
        ServiceTransacciones service;
      
        public TransaccionesController(ServiceTransacciones service)
        {
            this.service = service;
      
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Index()
        {
            String token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("LogOut", "Identity");
            }

            int user = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Transacciones> transacciones = await this.service.GetTransacciones();

            var json = HelperToolkit.SerializeJsonObject(transacciones);

            ViewData["USUARIO"] = User.FindFirstValue(ClaimTypes.Name);
            ViewData["ID"] = user;
            ViewData["json"] = json;
            return View(transacciones);
        }

        [HttpPost]
        public async Task<IActionResult> NuevaTransaccion(float cantidad, String tipoTransaccion, String Concepto)
        {
            String token = HttpContext.Session.GetString("token");
            String date = DateTime.Now.ToShortTimeString();
            DateTime fecha = Convert.ToDateTime(date);
            int IdUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await this.service.NuevaTransaccion(cantidad, IdUsuario, tipoTransaccion, Concepto);
            ViewData["MSG"] = "Transacción creada";
            return RedirectToAction("Index", "Transacciones");
        }

        public async Task<IActionResult> Delete(int idtransaccion)
        {
            String token = HttpContext.Session.GetString("token");
            await this.service.EliminarTransaccion(idtransaccion);
            TempData["MENSAJE"] = "Transaccion eliminada correctamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ModificarTransaccion(int idtransaccion, float cantidad, string tipoTransaccion, string concepto)
        {
            await this.service.ModificarTransaccion(idtransaccion, cantidad, tipoTransaccion, concepto);
            TempData["MENSAJE"] = "Transaccion Modificada correctamente";
            return RedirectToAction("Index");
        }
    }
}
