namespace Models;

public class Smartwatch : Device
{
    private int _batteryLevel;

    public int BatteryLevel { get; set; }

    public Smartwatch(string id, string name, bool isTurnedOn, int batteryLevel)
        : base(id, name, isTurnedOn)
    {
        BatteryLevel = batteryLevel;
    }
    

    public override void TurnMode()
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

    public override object GetInfo()
    {
        return new
        {
            Type = "Smartwatch",
            Id,
            Name,
            IsTurnedOn,
            BatteryLevel = _batteryLevel
        };
    }
}