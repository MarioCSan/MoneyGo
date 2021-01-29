using Microsoft.EntityFrameworkCore;
using MoneyGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Data
{
    public class TransaccionesContext: DbContext
    {
        public TransaccionesContext(DbContextOptions<TransaccionesContext> options) : base(options) { }

        public DbSet<Transacciones> transacciones { get; set; }
    }
}
