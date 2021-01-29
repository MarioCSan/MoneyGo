using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyGo.Models;

namespace MoneyGo.Repositories
{
    interface IRepositoryTransacciones
    {
        List<Transacciones> GetTransacciones(int idusuario);
    }
}
