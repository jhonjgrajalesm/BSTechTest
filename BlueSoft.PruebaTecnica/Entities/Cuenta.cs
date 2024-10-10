namespace BlueSoft.PruebaTecnica.Entities
{
    public class Cuenta
    {
        public Guid Id { get; set; }
        public Guid AhorradorId { get; set; }
        public string NroCuenta { get; set; }
        public decimal Saldo { get; set; }
        public string CiudadOrigen { get; set; }
    }
}
