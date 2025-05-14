namespace Models;

public class PersonalComputer : Device
{
    public string? OperationSystem { get; set; }
    public byte[] PcRowVersion { get; set; }


    public PersonalComputer(){}
    public PersonalComputer(string id, string name, bool isTurnedOn, byte[] originalVersion, string operationSystem)
        : base(id, name, isTurnedOn, originalVersion)
    {
        if (string.IsNullOrEmpty(operationSystem) && isTurnedOn)
            throw new Exception("EmptySystemException");
        OperationSystem = operationSystem;
    }

    public void TurnMode()
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

    public object GetInfo()
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