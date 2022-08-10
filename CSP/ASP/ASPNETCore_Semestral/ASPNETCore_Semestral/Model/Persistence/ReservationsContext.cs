using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model.Persistence
{
    public class ReservationsContext : DbContext
    {
        public ReservationsContext(DbContextOptions<ReservationsContext> options) : base(options)
        { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Reservation>().ToTable("Reservations");

            //modelBuilder.Entity<Room>()
            //     .HasMany(e => e.Reservations);
        }
    }
}
