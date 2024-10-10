using BlueSoft.PruebaTecnica.Entities;
using BlueSoft.PruebaTecnica.Enumeraciones;

namespace BlueSoft.PruebaTecnica.Interfaces
{
    public interface ICuentaService
    {
        Task<bool> Movimiento(Guid ahorradorIdOrigen, Guid cuentaIdOrigen, decimal amount, string Ubicacion, TransactionType transactionType);
        public IQueryable<Ahorrador> GetAhorradores();
        public IQueryable<Cuenta> GetCuentas();
        Task<List<TransaccionCliente>> ListadoClientes();
        Task<List<TransaccionCliente>> ClientesRetiroFueraCiudad();        
    }
}
