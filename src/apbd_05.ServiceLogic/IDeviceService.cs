using DTOs;
using Models;

namespace APBD_05.ServiceLogic;

public interface IDeviceService
{
    IEnumerable<DeviceDto> GetAllDevices();
    Device GetDeviceById(string id);
    
    void AddSmartwatch(Smartwatch device);
    Task<bool> ModifySmartwatch(Smartwatch device);
    
    void AddPersonalComputer(PersonalComputer device);
    Task<bool> ModifyPersonalComputer(PersonalComputer device);
    
    void AddEmbeddedDevice(EmbeddedDevice device);
    Task<bool> ModifyEmbeddedDevice(EmbeddedDevice device);
    
    Task<bool> RemoveDevice(string id);
}