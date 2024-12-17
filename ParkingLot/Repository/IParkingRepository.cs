using API.Entities;
using API.Models;

namespace API.Repository;

public interface IParkingRepository
{
    Task<ParkingSpot> FindAvailableSpotAsync(VehicleType type);
    Task<ParkedVehicle> FindParkedVehicleAsync(string licensePlate);
    Task<List<ParkingLevel>> GetAllLevelsWithSpotsAsync();
    Task<bool> AddLevelAsync(ParkingLevel level);
    Task SaveChangesAsync();
}
