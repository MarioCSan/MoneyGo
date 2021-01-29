using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Repositories
{
    interface IRepositoryTransacciones
    {
        List<Transacciones> GetTransacciones();
    }
}
