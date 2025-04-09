using Models;

namespace Logic;

public class DeviceManager
{
    private List<Device> devices = new List<Device>();
        private readonly int maxDevices = 15;

        public DeviceManager() { }
        
        public IEnumerable<Device> GetDevices()
        {
            return devices;
        }

        public Device GetDevice(string id)
        {
            return devices.FirstOrDefault(d => d.Id == id);
        }
        
        public void AddDevice(Device device)
        {
            if (devices.Count >= maxDevices)
                throw new Exception("Device storage is full.");
            devices.Add(device);
        }

        public void EditDeviceData(string deviceId, Device newDeviceData)
        {
            var device = GetDevice(deviceId);
            if (device == null)
                throw new Exception("Device not found.");

            if (device.GetType() != newDeviceData.GetType())
                throw new Exception("Device type mismatch.");

            device.Name = newDeviceData.Name;
            device.IsTurnedOn = newDeviceData.IsTurnedOn;

            if (device is Smartwatch sw && newDeviceData is Smartwatch newSw)
            {
                sw.BatteryLevel = newSw.BatteryLevel;
            }
            else if (device is PersonalComputer pc && newDeviceData is PersonalComputer newPc)
            {
                pc.OperationSystem = newPc.OperationSystem;
            }
            else if (device is EmbeddedDevice ed && newDeviceData is EmbeddedDevice newEd)
            {
                ed.IpAddress = newEd.IpAddress;
                ed.NetworkName = newEd.NetworkName;
            }
        }

        public void RemoveDevice(string deviceId)
        {
            var device = GetDevice(deviceId);
            if (device == null)
                throw new Exception("Device not found.");
            devices.Remove(device);
        }
}