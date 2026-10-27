using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Application.User;

namespace RoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> Get()
        => Ok(await userService.GetAsync());
}