namespace Models;

public class PersonalComputer : Device
{
    public string OperationSystem { get; set; }

    public PersonalComputer(string id, string name, bool isTurnedOn, string operationSystem)
        : base(id, name, isTurnedOn)
    {
        if (string.IsNullOrEmpty(operationSystem) && isTurnedOn)
            throw new Exception("EmptySystemException");
        OperationSystem = operationSystem;
    }

    public override void TurnMode()
    {
        if (IsTurnedOn)
        {
            IsTurnedOn = false;
        }
        else
        {
            if (!string.IsNullOrEmpty(OperationSystem))
                IsTurnedOn = true;
            else
                throw new Exception("EmptySystemException");
        }
    }

    public override object GetInfo()
    {
        return new
        {
            Type = "PersonalComputer",
            Id,
            Name,
            IsTurnedOn,
            OperationSystem
        };
    }
}