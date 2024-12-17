using API.DTOs;
using API.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IVehicleParking _vehicleParking;
        private readonly IParkingManagement _parkingManagement;

        public ParkingController(
            IVehicleParking vehicleParking,
            IParkingManagement parkingManagement)
        {
            _vehicleParking = vehicleParking;
            _parkingManagement = parkingManagement;
        }

        [HttpPost("park")]
        public async Task<ActionResult<ParkingReceiptDto>> ParkVehicle([FromBody] ParkVehicleRequestDto request)
        {
            try
            {
                var receipt = await _vehicleParking.ParkVehicleAsync(request);
                return Ok(receipt);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("unpark")]
        public async Task<ActionResult<UnparkingReceiptDto>> UnparkVehicle([FromBody] UnparkVehicleRequestDto request)
        {
            try
            {
                var receipt = await _vehicleParking.UnparkVehicleAsync(request);
                return Ok(receipt);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("availability")]
        public async Task<ActionResult<List<ParkingAvailabilityDto>>> GetAvailability()
        {
            var availability = await _parkingManagement.GetAvailabilityAsync();
            return Ok(availability);
        }

    }
}
