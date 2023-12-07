using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class AlquilerDb : DbContext
    {
        public AlquilerDb(DbContextOptions<AlquilerDb> options) : base(options) 
        {
        
        }
        public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    }
}
