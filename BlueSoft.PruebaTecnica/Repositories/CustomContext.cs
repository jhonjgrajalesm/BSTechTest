using BlueSoft.PruebaTecnica.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlueSoft.PruebaTecnica.Repositories
{
    public class CustomContext : DbContext
    {
        public CustomContext(DbContextOptions<CustomContext> options) : base(options) { }
        public DbSet<Ahorrador> Ahorradores { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }

        public DbSet<MovimientoCuenta> MovimientoCuentas { get; set; }
    }
}
