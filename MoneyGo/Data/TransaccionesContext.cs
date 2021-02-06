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
        public TransaccionesContext(DbContextOptions<TransaccionesContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Transacciones> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasAlternateKey(a => a.IdUsuario);
            modelBuilder.Entity<Transacciones>().HasAlternateKey(a => a.IdUsuario);
        }
    }
}
