Dependency Inversion Principle (DIP)
"High-level modules should not depend on low-level modules. Both should depend on abstractions."
DIP is followed because:
ParkingService depends on abstractions (interfaces) not concrete implementations
This allows easy swapping of implementations without changing ParkingService


Liskov Substitution Principle (LSP)
"Objects of a superclass should be replaceable with objects of its subclasses without breaking the application"
LSP is followed because:
Each derived class (Car, Motorcycle, Truck) maintains the contract of the base Vehicle class
Any code that works with Vehicle will work with any of its subtypes: