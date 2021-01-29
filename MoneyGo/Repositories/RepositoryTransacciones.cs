using MoneyGo.Data;
using MoneyGo.Helpers;
using MoneyGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Repositories
{
    public class RepositoryTransacciones : IRepositoryTransacciones
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
                           where datos.IdUsuario == idusuario select datos;
            
            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }

        public void InsertarUsuario(String nombreUsuario, String password, String Nombre, String email)
        {
            int maxId = 0; //Se necesita sacar el max(id) de la base de datos y sumarle 1;

            Usuario user = new Usuario();
            user.IdUsuario = maxId;
            user.Nombre = Nombre;
            user.NombreUsuario = nombreUsuario;
            user.Email = email;
            String salt = CypherService.GetSalt();
            user.Salt = salt;
            user.Password = CypherService.CifrarContenido(password, salt);
            
            this.context.Usuarios.Add(user);
            this.context.SaveChanges();

            this.MailService.SendEmailRegistro(email);
            
        }
    }
}
