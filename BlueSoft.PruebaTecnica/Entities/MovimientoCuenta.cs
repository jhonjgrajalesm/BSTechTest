namespace BlueSoft.PruebaTecnica.Entities
{
    public class MovimientoCuenta
    {
        public Guid Id { get; set; }
        public Guid CuentaOrigenId { get; set; }      
        public decimal Valor { get; set; }
        public bool Consignacion { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string Ubicacion { get; set; }
    }
}
