using API.Models;

namespace API.DTOs;
public record ParkingLevelDto(int Floor, int Capacity);
public record ParkVehicleRequestDto(string LicensePlate, VehicleType Type);
public record ParkingReceiptDto
{
    public string LicensePlate { get; init; }
    public DateTime EntryTime { get; init; }
    public int SpotNumber { get; init; }
    public int Floor { get; init; }
}
public record UnparkVehicleRequestDto(string LicensePlate);
public record UnparkingReceiptDto
{
    public string LicensePlate { get; init; }
    public DateTime EntryTime { get; init; }
    public DateTime ExitTime { get; init; }
    public decimal Fee { get; init; }
}
public record ParkingAvailabilityDto
{
    public int Floor { get; init; }
    public int TotalSpots { get; init; }
    public int AvailableSpots { get; init; }
    public Dictionary<VehicleType, int> AvailabilityByType { get; init; }
}
