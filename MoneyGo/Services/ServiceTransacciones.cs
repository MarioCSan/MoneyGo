using Microsoft.AspNetCore.Http;
using MoneyGo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGo.Services
{
    public class ServiceTransacciones
    {

        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;
        private IHttpContextAccessor http;
        
        public ServiceTransacciones(String url, IHttpContextAccessor http)
        {
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.http = http;
        }

       
        private async Task<T> CallApi<T>(String request)
        {
            String token = this.http.HttpContext.Session.GetString("token");
            using (HttpClient client = new HttpClient())
            {
                
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Transacciones>> GetTransacciones()
        {
            String request = "/api​/Transacciones";
            List<Transacciones> transacciones =
                await this.CallApi<List<Transacciones>>(request);
            return transacciones;
        }

        public async Task NuevaTransaccion(float cantidad, String tipoTransaccion, String concepto)
        {

            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Transacciones/NuevaTransaccion";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Transacciones trnsc = new Transacciones();

                trnsc.Cantidad = cantidad;
                trnsc.TipoTransaccion = tipoTransaccion;
                trnsc.Concepto = concepto;

                String json = JsonConvert.SerializeObject(trnsc);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }

        }

        public async Task<Transacciones> GetDataModificar(int idtransaccion)
        {
            String request = "/api​/Transacciones/GetTransaccion/" + idtransaccion;
            Transacciones transacciones =
                await this.CallApi<Transacciones>(request);
            return transacciones;

        }

        public async Task ModificarTransaccion(int idtransaccion, float cantidad, String tipoTransaccion, String concepto)
        {

            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Transacciones/Modificar/" + idtransaccion;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Transacciones trnsc = await GetDataModificar(idtransaccion);


                trnsc.Cantidad = cantidad;
                trnsc.TipoTransaccion = tipoTransaccion;
                trnsc.Concepto = concepto;

                String json = JsonConvert.SerializeObject(trnsc);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }

        }

        public async Task EliminarTransaccion(int idtransaccion)
        {
            String request = "/api/Transacciones/Eliminar/" + idtransaccion;
            await this.CallApi<Transacciones>(request);
        }

    }
}
