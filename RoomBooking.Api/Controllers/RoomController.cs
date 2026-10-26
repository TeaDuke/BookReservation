using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Application.Room;

namespace RoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController(IRoomService roomService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> Get([FromQuery] RoomFilter filter) 
        => Ok(await roomService.GetAsync(filter));
}