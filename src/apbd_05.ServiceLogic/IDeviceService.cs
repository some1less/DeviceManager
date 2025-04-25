using apbd_05.ServiceLogic;
using Models;

namespace APBD_05.ServiceLogic;

public interface IDeviceService
{
    IEnumerable<DeviceDto> GetAllDevices();
    Device GetDeviceById(string id);
    
    bool AddSmartwatch(Smartwatch device);
    bool ModifySmartwatch(string id, Smartwatch device);
    
    /*bool AddPersonalComputer(DeviceDto device);
    bool ModifyPersonalComputer(string id, DeviceDto device);
    
    bool AddEmbeddedDevice(DeviceDto device);
    bool ModifyEmbeddedDevice(string id, DeviceDto device);*/
    
    bool RemoveDevice(string id);
}