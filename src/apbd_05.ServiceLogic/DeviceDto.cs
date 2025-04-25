namespace apbd_05.ServiceLogic;

public class DeviceDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }
    
    public int?    BatteryLevel    { get; set; }
    public string? OperationSystem { get; set; }
    public string? IpAddress       { get; set; }
    public string? NetworkName     { get; set; }
}