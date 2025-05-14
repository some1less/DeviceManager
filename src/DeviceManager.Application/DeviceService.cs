using DeviceManager.Application;
using DeviceManager.DTOs;
using DeviceManager.DTOs.embeddeddevice;
using DeviceManager.DTOs.personalcomputer;
using DeviceManager.Infrastructure;
using Models;

namespace DeviceManager.Applicatio;

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
        List<DeviceDto> devicesDto = [];
        
        foreach (var device in devices)
        {
            var deviceDto = new DeviceDto
            {
                Id = device.Id,
                Name = device.Name,
                IsTurnedOn = device.IsTurnedOn
            };
            
            devicesDto.Add(deviceDto);
        }
        return devicesDto;
    }

    public Device GetDeviceById(string id)
    {
        var device = _deviceRepository.GetDeviceById(id);
        return device;
    }
    
    public SmartwatchDto AddSmartwatch(SmartwatchDto device)
    {
        Smartwatch sw = new Smartwatch
        {
            Id = device.Id,
            Name = device.Name,
            IsTurnedOn = device.IsTurnedOn,
            BatteryLevel = device.BatteryLevel
        };
        
        sw.Id = _deviceRepository.AddSmartwatch(sw);

        return new SmartwatchDto
        {
            Id = sw.Id,
            Name = sw.Name,
            IsTurnedOn = sw.IsTurnedOn,
            BatteryLevel = sw.BatteryLevel
        };
    }
    
    public PersonalComputerDto AddPersonalComputer(PersonalComputerDto device)
    {
        PersonalComputer pc = new PersonalComputer
        {
            Id = device.Id,
            Name = device.Name,
            IsTurnedOn = device.IsTurnedOn,
            OperationSystem = device.OperationSystem
        };

        pc.Id = _deviceRepository.AddPersonalComputer(pc);

        return new PersonalComputerDto
        {
            Id = pc.Id,
            Name = pc.Name,
            IsTurnedOn = pc.IsTurnedOn,
            OperationSystem = pc.OperationSystem
        };
    }
    
    public EmbeddedDeviceDto AddEmbeddedDevice(EmbeddedDeviceDto device)
    {
        EmbeddedDevice ed = new EmbeddedDevice
        {
            Id = device.Id,
            Name = device.Name,
            IsTurnedOn = device.IsTurnedOn,
            IpAddress = device.IpAddress,
            NetworkName = device.NetworkName
        };
        
        ed.Id = _deviceRepository.AddEmbeddedDevice(ed);

        return new EmbeddedDeviceDto
        {
            Id = ed.Id,
            Name = ed.Name,
            IsTurnedOn = ed.IsTurnedOn,
            IpAddress = ed.IpAddress,
            NetworkName = ed.NetworkName
        };
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