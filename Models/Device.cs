namespace Models;

public abstract class Device
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }

    protected Device(string id, string name, bool isTurnedOn)
    {
        Id = id;
        Name = name;
        IsTurnedOn = isTurnedOn;
    }

    public abstract void TurnMode();

    public abstract object GetInfo();
}