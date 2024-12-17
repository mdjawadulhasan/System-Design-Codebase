using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.DB;

public class ParkingDbContext : DbContext
{
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options) { }

    public DbSet<ParkingLevel> Levels { get; set; }
    public DbSet<ParkingSpot> Spots { get; set; }
    public DbSet<ParkedVehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingLevel>()
            .HasMany(l => l.ParkingSpots)
            .WithOne(s => s.Level)
            .HasForeignKey(s => s.LevelId);

        modelBuilder.Entity<ParkingSpot>()
            .HasOne(s => s.ParkedVehicle)
            .WithOne(v => v.ParkingSpot)
            .HasForeignKey<ParkedVehicle>(v => v.SpotId);
    }
}
