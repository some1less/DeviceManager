using DTOs;
using Models;

namespace Infrastructure;

public interface IDeviceRepository
{
    IEnumerable<DeviceDto> GetAllDevices();
    Device GetDeviceById(string id);
    
    string AddSmartwatch(Smartwatch device);
    string AddPersonalComputer(PersonalComputer device);
    string AddEmbeddedDevice(EmbeddedDevice device);
    Task<bool> ModifySmartwatch(Smartwatch device);

    Task<bool> ModifyPersonalComputer(PersonalComputer device);
    Task<bool> ModifyEmbeddedDevice(EmbeddedDevice device);
    
    Task<bool> RemoveDevice(string id);
}