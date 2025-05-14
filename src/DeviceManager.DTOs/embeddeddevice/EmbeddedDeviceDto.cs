namespace DeviceManager.DTOs.embeddeddevice;

public class EmbeddedDeviceDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }
    public string IpAddress {get;set;}
    public string NetworkName { get; set; }

}