namespace WebApi.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public string Color { get; set; }
        public string Placa { get; set; }
        public decimal PrecioAlquilerPorDia { get; set; }
        public bool Disponibilidad { get; set; }

    }
}
