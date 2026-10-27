using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Application.Reservation;
using RoomBooking.Api.Domain;

namespace RoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ReservationDto>>> GetAsync([FromQuery] ReservationFilter filter)
        => Ok(await reservationService.GetAsync(filter));
}