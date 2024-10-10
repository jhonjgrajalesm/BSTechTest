using BlueSoft.PruebaTecnica.Entities;
using BlueSoft.PruebaTecnica.Enumeraciones;
using BlueSoft.PruebaTecnica.Interfaces;
using BlueSoft.PruebaTecnica.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BlueSoft.PruebaTecnica.Services
{
    public class CuentaService : ICuentaService
    {

        private CustomContext customContext;

        public CuentaService(CustomContext customContext)
        {
            this.customContext = customContext;
        }

        public async Task<bool> Movimiento(Guid ahorradorIdOrigen, Guid cuentaIdOrigen, decimal amount, string Ubicacion, TransactionType transationType)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                if (amount <= 0)
                {
                    throw new Exception("El valor debe ser mayor de 0");
                }

                if (transationType == TransactionType.Retiro && this.customContext.Cuentas.Where(u => u.Id == cuentaIdOrigen).First().Saldo - amount < 0)
                {
                    throw new Exception("La cuenta no tiene saldo suficiente");
                }

                this.customContext.MovimientoCuentas.Add(new MovimientoCuenta()
                {
                    CuentaOrigenId = cuentaIdOrigen,
                    Consignacion = transationType == TransactionType.Consignacion ? true : false,
                    Valor = amount,
                    FechaTransaccion = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    Ubicacion = Ubicacion,
                });

                var corigen = this.customContext.Cuentas.Where(u => u.Id == cuentaIdOrigen).First();

                corigen.Saldo = transationType == TransactionType.Consignacion ? corigen.Saldo + amount : corigen.Saldo - amount;

                this.customContext.SaveChanges();

                transaction.Complete();
            }

            return true;
        }

        public IQueryable<Ahorrador> GetAhorradores()
        {
            return customContext.Ahorradores.AsQueryable();
        }

        public IQueryable<Cuenta> GetCuentas()
        {
            return customContext.Cuentas.AsQueryable();
        }

        public async Task<List<TransaccionCliente>> ListadoClientes()
        {
            var list = await (from m in customContext.MovimientoCuentas
                              join c in customContext.Cuentas on m.CuentaOrigenId equals c.Id
                              join a in customContext.Ahorradores on c.AhorradorId equals a.Id
                              select new TransaccionCliente
                              {
                                  Consignacion = m.Consignacion,
                                  FechaTransaccion = m.FechaTransaccion,
                                  NombreCliente = a.Nombre,
                                  NroCuenta = c.NroCuenta,
                                  Ubicacion = m.Ubicacion,
                                  Valor = m.Valor
                              }).ToListAsync();
            var orderedList = list
                .GroupBy(x => x.NombreCliente)
                .OrderByDescending(g => g.Count()) 
                .SelectMany(g => g) 
                .ToList();
            return orderedList;
        }

        public async Task<List<TransaccionCliente>> ClientesRetiroFueraCiudad()
        {
            var list = await (from m in customContext.MovimientoCuentas
                              join c in customContext.Cuentas on m.CuentaOrigenId equals c.Id
                              join a in customContext.Ahorradores on c.AhorradorId equals a.Id
                              where m.Valor > 1000000 && m.Ubicacion != c.CiudadOrigen
                              && !m.Consignacion 
                              select new TransaccionCliente
                              {
                                  Consignacion = m.Consignacion,
                                  FechaTransaccion = m.FechaTransaccion,
                                  NombreCliente = a.Nombre,
                                  NroCuenta = c.NroCuenta,
                                  Ubicacion = m.Ubicacion,
                                  Valor = m.Valor,
                                  UbicacionOrigenCuenta = c.CiudadOrigen
                              }).ToListAsync();
            return list;
        }
    }
}
