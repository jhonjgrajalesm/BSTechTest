using BlueSoft.PruebaTecnica.Entities;

namespace BlueSoft.PruebaTecnica.Repositories
{
    public static class DatabaseSeeder
    {
        public static void Seed(CustomContext context)
        {
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            // Check if there are any clients in the database. If not, seed the initial data.
            if (!context.Ahorradores.Any())
            {
                context.Ahorradores.AddRange(
                    new Ahorrador { Id = id, Nombre = "Jhon Grajales", Identificacion = "1128407538" },
                    new Ahorrador { Id = id2, Nombre = "Blue Software", Identificacion = "900988966" }
                );

                context.SaveChanges();
            }

            if (!context.Cuentas.Any())
            {
                context.Cuentas.AddRange(
                    new Cuenta { Id = Guid.NewGuid(), NroCuenta = "10115363907", AhorradorId = id, Saldo = 10000000, CiudadOrigen = "Medellin" },
                    new Cuenta { Id = Guid.NewGuid(), NroCuenta = "10115363904", AhorradorId = id, Saldo = 20000000, CiudadOrigen = "Medellin" },
                    new Cuenta { Id = Guid.NewGuid(), NroCuenta = "10115363906", AhorradorId = id2, Saldo = 100000000, CiudadOrigen = "Bogota" }
                );

                context.SaveChanges();
            }

            // Add more seeding logic here for other entities as needed.
        }
    }
}
