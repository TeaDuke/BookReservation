namespace RoomBooking.Api.Application.Equipment;

public record EquipmentDto(long Id, string Name, List<string> Rooms);