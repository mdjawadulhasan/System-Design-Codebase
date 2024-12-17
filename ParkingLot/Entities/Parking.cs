using API.Models;

namespace API.Entities;

public class ParkingLevel
{
    public int Id { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ParkingSpot> ParkingSpots { get; set; }
}

public class ParkingSpot
{
    public int Id { get; set; }
    public int SpotNumber { get; set; }
    public VehicleType VehicleType { get; set; }
    public bool IsOccupied { get; set; }
    public int LevelId { get; set; }
    public ParkingLevel Level { get; set; }
    public ParkedVehicle ParkedVehicle { get; set; }
}

public class ParkedVehicle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; }
    public VehicleType Type { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal? Fee { get; set; }
    public int SpotId { get; set; }
    public ParkingSpot ParkingSpot { get; set; }
}
