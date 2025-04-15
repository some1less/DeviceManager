using Models;

namespace Logic;

public class DeviceManager
{
        public static DeviceManager Instance { get; } = new DeviceManager();
        
        public Device GetDeviceById(string id)
        {
            return DeviceData.Devices.FirstOrDefault(d => d.Id.Equals(id));
        }
        
        public void AddDevice(Device device)
        {
            DeviceData.Devices.Add(device);
        }

        public void EditDeviceData(string deviceId, Device newDeviceData)
        {
            var device = GetDeviceById(deviceId);
            if (device == null)
                throw new Exception("Device not found.");

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
            var device = GetDeviceById(deviceId);
            if (device == null)
                throw new Exception("Device not found.");
            DeviceData.Devices.Remove(device);
        }
}