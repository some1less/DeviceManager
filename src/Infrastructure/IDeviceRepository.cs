using DTOs;
using Models;

namespace Infrastructure;

public interface IDeviceRepository
{
    IEnumerable<DeviceDto> GetAllDevices();
    Device GetDeviceById(string id);
    
    bool AddSmartwatch(Smartwatch device);
    bool ModifySmartwatch(string id, Smartwatch device);
    
    bool AddPersonalComputer(PersonalComputer device);
    bool ModifyPersonalComputer(string id, PersonalComputer device);
    
    bool AddEmbeddedDevice(EmbeddedDevice device);
    bool ModifyEmbeddedDevice(string id, EmbeddedDevice device);
    
    bool RemoveDevice(string id);
}