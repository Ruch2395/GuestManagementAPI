using GuestManagementAPI.Models;
using GuestManagementAPI.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;


namespace GuestManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }

        public DbSet<ReservationDBModel> Reservations { get; set; } = default!;
    }
}
