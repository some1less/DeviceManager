using APBD_05.ServiceLogic;
using DTOs;
using Infrastructure;
using Models;

namespace APBD_05.service;

public class DeviceService : IDeviceService
{ 
    private readonly IDeviceRepository _deviceRepository;
    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public IEnumerable<DeviceDto> GetAllDevices()
    {
        var devices = _deviceRepository.GetAllDevices();
        return devices;
    }

    public Device GetDeviceById(string id)
    {
        var device = _deviceRepository.GetDeviceById(id);
        return device;
    }
    
    public bool AddSmartwatch(Smartwatch device)
    {
        var result = _deviceRepository.AddSmartwatch(device);
        return result;
    }
    public bool ModifySmartwatch(string id, Smartwatch device)
    {
        var result = _deviceRepository.ModifySmartwatch(id, device);
        return result;
    }
    
     public bool AddPersonalComputer(PersonalComputer device)
     {
         var result = _deviceRepository.AddPersonalComputer(device);
         return result;
     }

     public bool ModifyPersonalComputer(string id, PersonalComputer device)
     {   
         var result = _deviceRepository.ModifyPersonalComputer(id, device);
         return result;
     }

    public bool AddEmbeddedDevice(EmbeddedDevice device)
    {
        var result = _deviceRepository.AddEmbeddedDevice(device);
        return result;
    }

     public bool ModifyEmbeddedDevice(string id, EmbeddedDevice device)
     {
         var result = _deviceRepository.ModifyEmbeddedDevice(id, device);
         return result;
     }

    public bool RemoveDevice(string id)
    {
        var result = _deviceRepository.RemoveDevice(id);
        return result;
    }
}