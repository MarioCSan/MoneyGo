using MoneyGo.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public async Task<String> GetToken(String useremail, String password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel login = new LoginModel
                {
                    UserEmail = useremail,
                    Password = password
                };
                String json = JsonConvert.SerializeObject(login);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                String request = "auth/login";
                HttpResponseMessage response = await client.PostAsync(request, content);

                if (response.IsSuccessStatusCode)
                {
                    String data = await response.Content.ReadAsStringAsync();
                    JObject jobject = JObject.Parse(data);

                    String token = jobject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
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

        private async Task<T> CallApi<T>(String request, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response = await client.GetAsync(request);
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

        public async Task<Usuario> GetDataUsuario(String token)
        {
            String request = "/api/Usuarios/GetDataUsuario";
            Usuario data = await this.CallApi<Usuario>(request, token);
            return data;
        }

        public async Task<Usuario> GetDataUsuario()
        {
            String request = "/api/Usuarios/GetDataUsuario";
            Usuario data = await this.CallApi<Usuario>(request);
            return data;
        }

        public async Task InsertarUsuario(String Nombre, String nombreUsuario, String password, String email)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/NuevoUsuario";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                byte[] pass = Encoding.ASCII.GetBytes(password);
                Usuario user = new Usuario();
                user.Nombre = Nombre;
                user.NombreUsuario = nombreUsuario;
                user.Password = pass;
                user.Email = email;

                String json = JsonConvert.SerializeObject(user);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }

        public async Task ModificarPassword(String password, String token)
        {

            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/ModificarPassword";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
            
                String json = JsonConvert.SerializeObject(password);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
  
        }

        public async Task<bool> BuscarEmail(String email)
        {
            String request = "/api/Usuarios/BuscarEmail/"+email;
            bool valido = await this.CallApi<bool>(request);
            return valido;
        }

        public async Task ModificarImagen(String imagen, String token)
        {

            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/ModificarImagen";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                String json = JsonConvert.SerializeObject(imagen);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }

        }

        public string GenerarTokenEmail()
        {
            Random rnd = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 16).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
