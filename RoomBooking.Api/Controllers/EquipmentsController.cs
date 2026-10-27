using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Application.Equipment;

namespace RoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentsController(IEquipmentService equipmentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EquipmentDto>>> Get()
        => Ok(await equipmentService.GetAsync());
}   