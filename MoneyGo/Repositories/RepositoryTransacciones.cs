using MoneyGo.Data;
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

        public RepositoryTransacciones(TransaccionesContext context)
        {
            this.context = context;
        }

        public List<Transacciones> GetTransacciones(int idusuario)
        {
            var consulta = from datos in this.context.transacciones 
                           where datos.IdUsuario == idusuario select datos;
            
            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }

    }
}
