using Microsoft.AspNetCore.Mvc;
using GuestManagementAPI.Services;
using GuestManagementAPI.DTOs;

namespace GuestManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var result = await _reservationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? status,
            [FromQuery] string? roomNumber,
            [FromQuery] string? sortBy,
            [FromQuery] string? order = "asc")
        {
            var results = await _reservationService.GetAllAsync(status, roomNumber, sortBy, order);
            return Ok(results);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reservationService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateReservationDto dto)
        {
            var result = await _reservationService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/check-in")]
        public async Task<IActionResult> CheckIn(Guid id)
        {
            await _reservationService.CheckInAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:guid}/check-out")]
        public async Task<IActionResult> CheckOut(Guid id)
        {
            await _reservationService.CheckOutAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _reservationService.CancelAsync(id);
            return NoContent();
        }
    }
}


