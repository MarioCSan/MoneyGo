using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Services
{
    public class ServiceSession
    {
        private IHttpContextAccessor httpContext;
    
        public ServiceSession(IHttpContextAccessor http)
        {
            this.httpContext = http;
        }

        public void setSession(String nombre, String token)
        {
            this.httpContext.HttpContext.Session.SetString(nombre, token);
        }

        public String GetSession(String nombre)
        {
            return this.httpContext.HttpContext.Session.GetString(nombre);
        }
    }
}
