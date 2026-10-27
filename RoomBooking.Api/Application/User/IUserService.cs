namespace RoomBooking.Api.Application.User;

public interface IUserService
{
    Task<List<UserDto>> GetAsync();
}