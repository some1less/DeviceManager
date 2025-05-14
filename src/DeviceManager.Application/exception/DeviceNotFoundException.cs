namespace DeviceManager.Application.exception;

public class DeviceNotFoundException : Exception
{
    public DeviceNotFoundException(string message)
        : base(message)
    {
    }
}