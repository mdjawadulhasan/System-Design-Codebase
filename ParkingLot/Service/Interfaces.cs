using API.DTOs;
using API.Models;

namespace API.Service;

public interface IVehicleParking
{
    Task<ParkingReceiptDto> ParkVehicleAsync(ParkVehicleRequestDto request);
    Task<UnparkingReceiptDto> UnparkVehicleAsync(UnparkVehicleRequestDto request);
}

public interface IParkingManagement
{
    Task<List<ParkingAvailabilityDto>> GetAvailabilityAsync();
    Task<bool> AddLevelAsync(ParkingLevelDto levelDto);
}

public interface IFeeCalculator
{
    decimal CalculateFee(DateTime entryTime, DateTime exitTime, VehicleType type);
}

public class HourlyFeeCalculator : IFeeCalculator
{
    public decimal CalculateFee(DateTime entryTime, DateTime exitTime, VehicleType type)
    {
        var duration = exitTime - entryTime;
        var hours = Math.Ceiling(duration.TotalHours);
        var baseRate = type switch
        {
            VehicleType.Car => 2.0m,
            VehicleType.Motorcycle => 1.0m,
            VehicleType.Truck => 3.0m,
            _ => throw new ArgumentException("Invalid vehicle type")
        };
        return baseRate * (decimal)hours;
    }
}