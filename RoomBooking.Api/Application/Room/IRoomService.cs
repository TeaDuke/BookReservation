namespace RoomBooking.Api.Application.Room;

public interface IRoomService
{
    Task<List<RoomDto>> GetAsync(RoomFilter filter);
}