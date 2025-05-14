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
    
    public void AddSmartwatch(Smartwatch device)
    {
        _deviceRepository.AddSmartwatch(device);
    }
    
    public void AddPersonalComputer(PersonalComputer device)
    { 
        _deviceRepository.AddPersonalComputer(device);
    }
    
    public void AddEmbeddedDevice(EmbeddedDevice device)
    {
        _deviceRepository.AddEmbeddedDevice(device);
    }
    
    public Task<bool> ModifySmartwatch(Smartwatch sw)
    {
        var result = _deviceRepository.ModifySmartwatch(sw);
        return result;
    }

    public Task<bool> ModifyPersonalComputer(PersonalComputer pc)
    {
        var result = _deviceRepository.ModifyPersonalComputer(pc);
        return result;
    }
    
    public Task<bool> ModifyEmbeddedDevice(EmbeddedDevice ed)
     {
         var result = _deviceRepository.ModifyEmbeddedDevice(ed);
         return result;
     }
    
    public Task<bool> RemoveDevice(string id)
    {
        var result = _deviceRepository.RemoveDevice(id);
        return result;
    }
}