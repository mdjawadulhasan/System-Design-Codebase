namespace API.Models;

public abstract class Vehicle
{
    public string LicensePlate { get; set; }
    public abstract VehicleType Type { get; }
    public abstract decimal BaseRate { get; }
}

public class Car : Vehicle
{
    public override VehicleType Type => VehicleType.Car;
    public override decimal BaseRate => 2.0m;
}

public class Motorcycle : Vehicle
{
    public override VehicleType Type => VehicleType.Motorcycle;
    public override decimal BaseRate => 1.0m;
}

public class Truck : Vehicle
{
    public override VehicleType Type => VehicleType.Truck;
    public override decimal BaseRate => 3.0m;
}

public enum VehicleType { Car = 1, Motorcycle = 2, Truck = 3 }
