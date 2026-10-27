using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Application.Reservation;
using RoomBooking.Api.Domain;

namespace RoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ReservationDto>>> GetAll([FromQuery] ReservationFilter filter)
        => Ok(await reservationService.GetAllAsync(filter));

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetById([FromRoute] long id)
        => Ok(await reservationService.GetByIdAsync(id));
    

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationRequest request)
    {
        var created = await reservationService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:long}/cancel")]
    public async Task<ActionResult<ReservationDto>> Cancel(long id)
        => Ok(await reservationService.CancelAsync(id));

    [HttpPost("{id:long}/end")]
    public async Task<ActionResult<ReservationDto>> End(long id)
        => Ok(await reservationService.EndEarlyAsync(id));
}