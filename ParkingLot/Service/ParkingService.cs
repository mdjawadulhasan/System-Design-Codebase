using API.DTOs;
using API.Entities;
using API.Models;
using API.Repository;

namespace API.Service;

public class ParkingService : IVehicleParking, IParkingManagement
{
    private readonly IParkingRepository _repository;
    private readonly IFeeCalculator _feeCalculator;
    private readonly ILogger<ParkingService> _logger;

    public ParkingService(
        IParkingRepository repository,
        IFeeCalculator feeCalculator,
        ILogger<ParkingService> logger)
    {
        _repository = repository;
        _feeCalculator = feeCalculator;
        _logger = logger;
    }

    public async Task<ParkingReceiptDto> ParkVehicleAsync(ParkVehicleRequestDto request)
    {
        var spot = await _repository.FindAvailableSpotAsync(request.Type);
        if (spot == null)
            throw new ApplicationException("No available spots for this vehicle type");

        var parkedVehicle = new ParkedVehicle
        {
            LicensePlate = request.LicensePlate,
            Type = request.Type,
            EntryTime = DateTime.UtcNow,
            SpotId = spot.Id
        };

        spot.IsOccupied = true;
        spot.ParkedVehicle = parkedVehicle;
        await _repository.SaveChangesAsync();

        return new ParkingReceiptDto
        {
            LicensePlate = request.LicensePlate,
            EntryTime = parkedVehicle.EntryTime,
            SpotNumber = spot.SpotNumber,
            Floor = spot.Level.Floor
        };
    }

    public async Task<UnparkingReceiptDto> UnparkVehicleAsync(UnparkVehicleRequestDto request)
    {
        var vehicle = await _repository.FindParkedVehicleAsync(request.LicensePlate);
        if (vehicle == null)
            throw new ApplicationException("Vehicle not found in parking lot");

        var exitTime = DateTime.UtcNow;
        var fee = _feeCalculator.CalculateFee(vehicle.EntryTime, exitTime, vehicle.Type);

        vehicle.ExitTime = exitTime;
        vehicle.Fee = fee;
        vehicle.ParkingSpot.IsOccupied = false;

        await _repository.SaveChangesAsync();

        return new UnparkingReceiptDto
        {
            LicensePlate = vehicle.LicensePlate,
            EntryTime = vehicle.EntryTime,
            ExitTime = exitTime,
            Fee = fee
        };
    }

    public async Task<List<ParkingAvailabilityDto>> GetAvailabilityAsync()
    {
        var levels = await _repository.GetAllLevelsWithSpotsAsync();

        return levels.Select(l => new ParkingAvailabilityDto
        {
            Floor = l.Floor,
            TotalSpots = l.ParkingSpots.Count,
            AvailableSpots = l.ParkingSpots.Count(s => !s.IsOccupied),
            AvailabilityByType = l.ParkingSpots
                .Where(s => !s.IsOccupied)
                .GroupBy(s => s.VehicleType)
                .ToDictionary(g => g.Key, g => g.Count())
        }).ToList();
    }

    public async Task<bool> AddLevelAsync(ParkingLevelDto levelDto)
    {
        var level = new ParkingLevel
        {
            Floor = levelDto.Floor,
            Capacity = levelDto.Capacity,
            CreatedAt = DateTime.UtcNow,
            ParkingSpots = GenerateParkingSpots(levelDto.Capacity)
        };

        await _repository.AddLevelAsync(level);
        await _repository.SaveChangesAsync();
        return true;
    }

    private List<ParkingSpot> GenerateParkingSpots(int capacity)
    {
        var spots = new List<ParkingSpot>();
        int carSpots = (int)(capacity * 0.6);
        int bikeSpots = (int)(capacity * 0.3);
        int truckSpots = capacity - carSpots - bikeSpots;
        int spotNumber = 1;

        foreach (var (type, count) in new[]
        {
            (VehicleType.Car, carSpots),
            (VehicleType.Motorcycle, bikeSpots),
            (VehicleType.Truck, truckSpots)
        })
        {
            for (int i = 0; i < count; i++)
            {
                spots.Add(new ParkingSpot
                {
                    SpotNumber = spotNumber++,
                    VehicleType = type,
                    IsOccupied = false
                });
            }
        }

        return spots;
    }
}