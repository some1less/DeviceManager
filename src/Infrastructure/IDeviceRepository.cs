using DTOs;
using Models;

namespace Infrastructure;

public interface IDeviceRepository
{
    IEnumerable<DeviceDto> GetAllDevices();
    Device GetDeviceById(string id);
    
    void AddSmartwatch(Smartwatch device);
    void AddPersonalComputer(PersonalComputer device);
    void AddEmbeddedDevice(EmbeddedDevice device);
    Task<bool> ModifySmartwatch(Smartwatch device);

    Task<bool> ModifyPersonalComputer(PersonalComputer device);
    Task<bool> ModifyEmbeddedDevice(EmbeddedDevice device);
    
    Task<bool> RemoveDevice(string id);
}