using Microsoft.AspNetCore.Mvc;
using Logic;
using Models;

namespace APBD_05;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly DeviceManager _deviceManager;

    public DevicesController(DeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }

    
    [HttpGet]
    public IResult GetDevices()
    {
        var list = _deviceManager.GetDevices()
            .Select(d => new 
            {
                d.Id,
                d.Name,
                d.IsTurnedOn
            });
        return Results.Ok(list);
    }

    [HttpGet("/{id}")]
    public IResult GetDevice(string id)
    {
        try
        {
            return Results.Ok(_deviceManager.GetDeviceById(id));
        }
        catch
        {
            return Results.NotFound();
        }
    }

    [HttpPost("/SmartWatch")]
    public IResult AddDevice(Smartwatch device)
    {
        _deviceManager.AddDevice(device);
        return Results.Ok(device);
    }
    [HttpPost("/PersonalComputer")]
    public IResult AddDevice(PersonalComputer device)
    {
        _deviceManager.AddDevice(device);
        return Results.Ok(device);
    }
    [HttpPost("/EmbeddedDevice")]
    public IResult AddDevice(EmbeddedDevice device)
    {
        _deviceManager.AddDevice(device);
        return Results.Ok(device);
    }
    
    [HttpPut("/SmartWatch/{id}")]
    public IResult UpdateSmartWatch(string id, [FromBody] Smartwatch device)
    {
        _deviceManager.EditDeviceData(id, device);
        try
        {
            return Results.Ok(_deviceManager.GetDeviceById(id));
        }
        catch
        {
            return Results.NotFound();
        }
    }
    
    [HttpPut("/PersonalComputer/{id}")]
    public IResult UpdatePersonalComputer(string id, [FromBody] PersonalComputer device)
    {
        _deviceManager.EditDeviceData(id, device);
        try
        {
            return Results.Ok(_deviceManager.GetDeviceById(id));
        }
        catch
        {
            return Results.NotFound();
        }
    }
    
    [HttpPut("/EmbeddedDevice/{id}")]
    public IResult UpdateEmbeddedDevice(string id, [FromBody] EmbeddedDevice device)
    {
        _deviceManager.EditDeviceData(id, device);
        try
        {
            return Results.Ok(_deviceManager.GetDeviceById(id));
        }
        catch
        {
            return Results.NotFound();
        }
    }
    
    [HttpDelete("/{id}")]
    public IResult DeleteDevice(string id)
    {
        try
        {
            _deviceManager.RemoveDevice(id);
            return Results.Ok();
        }
        catch
        {
            return Results.NotFound();
        }
        
    }
}