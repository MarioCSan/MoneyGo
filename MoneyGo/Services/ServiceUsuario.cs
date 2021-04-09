using MoneyGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MoneyGo.Services
{
    public class ServiceUsuario
    {

        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;


        public ServiceUsuario(String url)
        {
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<T> CallApi<T>(String request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
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

        public async Task<Usuario> GetDataUsuario()
        {
            String request = "/api/Usuarios/GetDataUsuario";
            Usuario data = await this.CallApi<Usuario>(request);
            return data;
        }

    }
}
