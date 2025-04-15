using Models;

namespace Logic;

public class DeviceData
{
    
    public static List<Device> Devices { get; set; } = [];
    public static void Load()
    {
        Devices = [
        new Smartwatch("SW-1", "Apple Watch", true, 80),
        new PersonalComputer("PC-1", "Dell Inspiron", false, "Windows 10"),
        new PersonalComputer("PC-2", "HP Pavilion", true, "Windows 11"),
        new EmbeddedDevice("ED-1", "Raspberry Pi", false, "192.168.1.100", "MD Ltd. Network")
        ];
    }
}