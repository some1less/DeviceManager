using System.Text.RegularExpressions;

namespace Models;

public class EmbeddedDevice : Device
{
    private string _ipAddress;
    public string IpAddress
    {
        get { return _ipAddress; }
        set
        {
            if (!IsValidIpAddress(value))
                throw new ArgumentException("Invalid IP Address");
            _ipAddress = value;
        }
    }

    public string NetworkName { get; set; }

    public EmbeddedDevice(){}
    public EmbeddedDevice(string id, string name, bool isTurnedOn, string ipAddress, string networkName)
        : base(id, name, isTurnedOn)
    {
        IpAddress = ipAddress;
        NetworkName = networkName;
        if (!NetworkName.Contains("MD Ltd.") && isTurnedOn)
            throw new Exception("ConnectionException");
    }

    public override void TurnMode()
    {
        if (IsTurnedOn)
        {
            IsTurnedOn = false;
        }
        else
        {
            if (NetworkName.Contains("MD Ltd."))
                IsTurnedOn = true;
            else
                throw new Exception("ConnectionException");
        }
    }

    public override object GetInfo()
    {
        return new
        {
            Type = "EmbeddedDevice",
            Id,
            Name,
            IsTurnedOn,
            IpAddress,
            NetworkName
        };
    }

    private bool IsValidIpAddress(string ipAddress)
    {
        string pattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";
        return Regex.IsMatch(ipAddress, pattern);
    }
}