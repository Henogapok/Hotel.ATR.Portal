using Microsoft.EntityFrameworkCore;

namespace Hotel.ATR.Portal
{
    public class HotelAtrContext : DbContext
    {
        public HotelAtrContext(DbContextOptions<HotelAtrContext> options) : base(options)
        {

        }
        public DbSet<Models.Room> Rooms { get; set; }
        public DbSet<Models.Client> Clients { get; set; }
    }
}
