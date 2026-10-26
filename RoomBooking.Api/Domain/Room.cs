using System.Collections;

namespace RoomBooking.Api.Domain;

public class Room
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int Floor  { get; set; }
    public List<Equipment> Equipments { get; set; } = [];
    public bool IsActive { get; set; }
}