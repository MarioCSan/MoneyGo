using Microsoft.AspNetCore.Http;
using MoneyGo.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGo.Services
{
    public class ServiceUsuario
    {

        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;
        private IHttpContextAccessor http;

        public ServiceUsuario(String url, IHttpContextAccessor http)
        {
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.http = http;
        }

        #region llamadas api
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
                String request = this.UriApi + "/api/auth/login";
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
            String token = this.http.HttpContext.Session.GetString("token");
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

        #endregion

        public async Task<Usuario> GetDataUsuario(int idusuario)
        {
            String request = "/api/Usuarios/GetDataUsuario/"+idusuario;
            Usuario data = await this.CallApi<Usuario>(request);
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
                String request = "api/Usuarios/NuevoUsuario";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                byte[] pass = Encoding.ASCII.GetBytes(password);
                Usuario user = new Usuario();
                user.IdUsuario = 0;
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

        public async Task ModificarPassword(String password)
        {
            
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/ModificarPassword";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                byte[] bytes = Encoding.ASCII.GetBytes(password);
                Usuario user = await this.GetDataUsuario();
                user.Password = bytes;

                String json = JsonConvert.SerializeObject(user);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
  
        }

        public async Task<bool> BuscarEmail(String email)
        {
            String request = "api/Usuarios/BuscarEmail/"+email;
            bool valido = await this.CallApi<bool>(request);
            return valido;
        }

        public async Task ModificarImagen(String imagen)
        {

            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/ModificarImagen";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Usuario user = await this.GetDataUsuario();
                user.ImagenUsuario = imagen;
                String json = JsonConvert.SerializeObject(user);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }

        }

        public async Task<Usuario> GetUsuarioEmail(String email)
        {
            String request = "/api/Usuarios/GetUsuarioEmail/"+email;
            Usuario data = await this.CallApi<Usuario>(request);
            return data;
        }

        public string GenerarTokenEmail()
        {
            Random rnd = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 16).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public async Task<Usuario> ValidarUsuario(String email, string password)
        {
            String request = "/api/Usuarios/GetDataUsuario";
            Usuario data = await this.CallApi<Usuario>(request);
            return data;
        }
    }
}
