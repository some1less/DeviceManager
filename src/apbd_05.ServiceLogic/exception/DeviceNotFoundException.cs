namespace apbd_05.ServiceLogic.exception;

public class DeviceNotFoundException : Exception
{
    public DeviceNotFoundException(string message)
        : base(message)
    {
    }
}