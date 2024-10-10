namespace BlueSoft.PruebaTecnica.Entities
{
    public class TransaccionCliente
    {
        public string NombreCliente { get; set; }
        public string NroCuenta { get; set; }
        public decimal Valor { get; set; }
        public bool Consignacion { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string Ubicacion { get; set; }
        public string UbicacionOrigenCuenta { get; set; }
    }
}
