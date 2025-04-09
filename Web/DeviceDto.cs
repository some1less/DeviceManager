using System.ComponentModel.DataAnnotations;

namespace APBD_05;

public class DeviceDto
{
    [Required]
    [MaxLength(10)]
    public string Id { get; set; }  
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }
        
    public int BatteryLevel { get; set; }
    
    public string? OperationSystem { get; set; }
        
    public string IpAddress { get; set; }
    public string NetworkName { get; set; }
}