namespace RoomBooking.Api.Application.Equipment;

public interface IEquipmentService
{
    Task<List<EquipmentDto>> GetAsync();
}