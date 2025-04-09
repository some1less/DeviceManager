namespace Models;

public class Smartwatch : Device
{
    private int _batteryLevel;
    

    public Smartwatch(string id, string name, bool isTurnedOn, int batteryLevel)
        : base(id, name, isTurnedOn)
    {
        _batteryLevel = batteryLevel;
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