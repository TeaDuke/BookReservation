namespace RoomBooking.Api.Application.Room;

public class RoomFilter
{
    public int? MinCapacity { get; set; }
    public int? Floor { get; set; }
    public List<long>? EquipmentIds { get; set; }
}   