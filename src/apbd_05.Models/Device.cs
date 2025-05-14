namespace Models;

public class Device
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }
    public byte[] OriginalVersion { get; set; }
    
    public Device(){}
    public Device(string id, string name, bool isTurnedOn, byte[] originalVersion)
    {
        Id = id;
        Name = name;
        IsTurnedOn = isTurnedOn;
        OriginalVersion = originalVersion;
    }

    public void TurnMode()
    {
        if (IsTurnedOn)
        {
            IsTurnedOn = false;
        }
        else
        {
            IsTurnedOn = true;
        }
    }

    public object GetInfo()
    {
        return new
        {
            Type = "Device",
            Id,
            Name,
            IsTurnedOn
        };
    }
}