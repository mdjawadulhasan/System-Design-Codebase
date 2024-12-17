using API.DB;
using API.Entities;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class ParkingRepository : IParkingRepository
{
    private readonly ParkingDbContext _context;

    public ParkingRepository(ParkingDbContext context) => _context = context;

    public async Task<ParkingSpot> FindAvailableSpotAsync(VehicleType type) =>
        await _context.Spots
            .Include(s => s.Level)
            .FirstOrDefaultAsync(s => s.VehicleType == type && !s.IsOccupied);

    public async Task<ParkedVehicle> FindParkedVehicleAsync(string licensePlate) =>
        await _context.Vehicles
            .Include(v => v.ParkingSpot)
            .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate && !v.ExitTime.HasValue);

    public async Task<List<ParkingLevel>> GetAllLevelsWithSpotsAsync() =>
        await _context.Levels.Include(l => l.ParkingSpots).ToListAsync();

    public async Task<bool> AddLevelAsync(ParkingLevel level)
    {
        await _context.Levels.AddAsync(level);
        return true;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}