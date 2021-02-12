using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Filters
{
    public class AuthorizeUsuariosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = this.RedirectToRoute("Identity", "Login");
            } else
            {
                context.Result = this.RedirectToRoute("Transacciones", "Index");
            }
        }

        private RedirectToRouteResult RedirectToRoute(String controller, String action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(new
            {
                controller = controller,
                action = action
            });

            RedirectToRouteResult redirect = new RedirectToRouteResult(ruta);
            return redirect;
        }
    }
}
