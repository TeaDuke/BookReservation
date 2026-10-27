namespace RoomBooking.Api.Application.Room;

public record RoomDto(long Id, string Name, int Capacity, int Floor, bool IsActive, List<string> Equipments)
{
    
}