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

        #region management

        public Usuario getDataUsuario(int id)
        {
            return this.context.Usuarios.Where(z => z.IdUsuario == id).FirstOrDefault();
        }

        public void UpdateImagen(int idusuario, String imagen)
        {
            Usuario user = this.getDataUsuario(idusuario);
            user.ImagenUsuario = imagen;
            this.context.SaveChanges();
        }

        #endregion

        #region transacciones
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

        

        public Transacciones BuscarTransacciones(int IdTransaccion)
        {
            return this.context.Transacciones.Where(z=> z.IdTransaccion == IdTransaccion).FirstOrDefault();
        }

        public void NuevaTransaccion(int IdUsuario, float Cantidad, String Tipo, String Concepto, DateTime Fecha)
        {
            var consulta = from datos in this.context.Transacciones
                           select datos.IdTransaccion;


            int maxId = consulta.Max();

            Transacciones trnsc = new Transacciones();
            trnsc.IdTransaccion = maxId + 1;
            trnsc.IdUsuario = IdUsuario;
            trnsc.Cantidad = Cantidad;
            trnsc.TipoTransaccion = Tipo;
            trnsc.Concepto = Concepto;
            trnsc.FechaTransaccion = Fecha;
            this.context.Add(trnsc);
            this.context.SaveChanges();

        }

        public void EliminarTransaccion(int idtransaccion)
        {
            //RGPD.¿Se que almacenar los datos X tiempo?¿Necesario campo extra a nulo o booleano para que no se muestre?
            Transacciones trnsc = this.BuscarTransacciones(idtransaccion);
            this.context.Transacciones.Remove(trnsc);
            this.context.SaveChanges();
        }


        #endregion

        #region usuariosLogin

        //Storedprocedure para el alta de usuario??
        public void InsertarUsuario(String nombreUsuario, String password, String Nombre, String email)
        {

            var consulta = from datos in this.context.Usuarios
                           select datos.IdUsuario;

            int maxId = consulta.Max() + 1;

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
                if (respuesta)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}