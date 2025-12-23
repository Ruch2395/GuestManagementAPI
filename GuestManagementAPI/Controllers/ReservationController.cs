using GuestManagementAPI.Models;
using GuestManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuestManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController(IReservationService _reservationService) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Create(ReservationRequest request, CancellationToken ct)
        {
            var dto = request.ToDomain();
            var result = await _reservationService.CreateAsync(dto, ct);
           var response = result.ToModel();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? status,
            [FromQuery] string? roomNumber,
            [FromQuery] string? sortBy,
            [FromQuery] string? order, CancellationToken ct)
        {
            order = order ?? "asc";
            var results = await _reservationService.GetAllAsync(status, roomNumber, sortBy, order, ct);
            return Ok(results.ToModel());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _reservationService.GetByIdAsync(id, ct);
            return Ok(result.ToModel());
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateReservation req, CancellationToken ct)
        {
            var dto = req.ToDomain();
            var result = await _reservationService.UpdateAsync(id, dto, ct);
            return Ok(result.ToModel());
        }

        [HttpPatch("{id:guid}/check-in")]
        public async Task<IActionResult> CheckIn(Guid id, CancellationToken ct)
        {
            await _reservationService.CheckInAsync(id, ct);
            return NoContent();
        }

        [HttpPatch("{id:guid}/check-out")]
        public async Task<IActionResult> CheckOut(Guid id, CancellationToken ct)
        {
            await _reservationService.CheckOutAsync(id, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
        {
            await _reservationService.CancelAsync(id, ct);
            return NoContent();
        }
    }
}


