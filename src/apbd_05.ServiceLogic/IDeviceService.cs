using DTOs;
using DTOs.embeddeddevice;
using DTOs.personalcomputer;
using Models;

namespace APBD_05.ServiceLogic;

public interface IDeviceService
{
    IEnumerable<DeviceDto> GetAllDevices();
    Device GetDeviceById(string id);
    
    SmartwatchDto AddSmartwatch(SmartwatchDto device);
    Task<bool> ModifySmartwatch(Smartwatch device);
    
    PersonalComputerDto AddPersonalComputer(PersonalComputerDto device);
    Task<bool> ModifyPersonalComputer(PersonalComputer device);
    
    EmbeddedDeviceDto AddEmbeddedDevice(EmbeddedDeviceDto device);
    Task<bool> ModifyEmbeddedDevice(EmbeddedDevice device);
    
    Task<bool> RemoveDevice(string id);
}