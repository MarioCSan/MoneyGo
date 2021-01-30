using MoneyGo.Data;
using MoneyGo.Helpers;
using MoneyGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Repositories
{
    public class RepositoryTransacciones
    {
        TransaccionesContext context;
        MailService MailService;

        public RepositoryTransacciones(TransaccionesContext context, MailService MailService)
        {
            this.context = context;
            this.MailService = MailService;
        }

        public List<Transacciones> GetTransacciones(int idusuario)
        {
            var consulta = from datos in this.context.Transacciones
                           where datos.IdUsuario == idusuario
                           select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }

        public void NuevaTransaccion(int IdUsuario, float Cantidad, String Tipo, DateTime Fecha)
        {
            var consulta = from datos in this.context.Transacciones
                           select datos.IdUsuario;


            int maxId = consulta.Max();

            Transacciones trnsc = new Transacciones();
            trnsc.IdTransaccion = maxId + 1;
            trnsc.IdUsuario = IdUsuario;
            trnsc.TipoTransaccion = Tipo;
            trnsc.FechaTransaccion = Fecha;


        }

        public void InsertarUsuario(String nombreUsuario, String password, String Nombre, String email)
        {

            var consulta = from datos in this.context.Usuarios
                           select datos.IdUsuario;

            int maxId = consulta.Max();

            Usuario user = new Usuario();
            user.IdUsuario = maxId + 1;
            user.Nombre = Nombre;
            user.NombreUsuario = nombreUsuario;
            user.Email = email;
            String salt = CypherService.GetSalt();
            user.Salt = salt;
            user.Password = CypherService.CifrarContenido(password, salt);

            this.context.Usuarios.Add(user);
            this.context.SaveChanges();

            this.MailService.SendEmailRegistro(email, Nombre);

        }

        public Usuario ValidarUsuario(String email, String password)
        {
            Usuario user = this.context.Usuarios.Where(z => z.Email == email).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            else
            {
                String salt = user.Salt;
                byte[] passbbdd = user.Password;
                byte[] passtmp = CypherService.CifrarContenido(password, salt);
                // comparar array bytes[]
                bool respuesta =
                HelperToolkit.CompararArrayBytes(passbbdd, passtmp);
                if (respuesta == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
