using apbd_05.ServiceLogic;
using APBD_05.ServiceLogic;
using Microsoft.Data.SqlClient;
using Models;

namespace APBD_05.service;

public class DeviceService : IDeviceService
{ 
    private readonly string _connectionString;

    public DeviceService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<DeviceDto> GetAllDevices()
    {
        List<DeviceDto> devices = [];
        
        const string sql = "select * from device";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(sql, connection);
            
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var deviceRow = new DeviceDto
                        {
                            DeviceId = reader.GetString(0),
                            DeviceName = reader.GetString(1),
                            IsTurnedOn = reader.GetBoolean(2),
                        };

                        devices.Add(deviceRow);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return devices;
        }
    }
    
    public bool AddSmartwatch(Smartwatch device)
    {
        const string insertString =
            "INSERT INTO Smartwatches (Id, Name, IsTurnedOn, BatteryLevel) VALUES (@Id, @Name, @IsTurnedOn, @BatteryLevel)";
        
        int rowsAffected = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(insertString, connection);
            command.Parameters.AddWithValue("@Id", device.Id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command.Parameters.AddWithValue("@BatteryLevel", device.BatteryLevel);
            
            connection.Open();
            
            rowsAffected = command.ExecuteNonQuery();
        }
        
        return rowsAffected != -1;

    }
    public bool ModifySmartwatch(string id, Smartwatch device)
    {
        const string updateString = "UPDATE Smartwatches " + 
                                    "SET Name = @Name, IsTurnedOn = @IsTurnedOn, BatteryLevel = @BatteryLevel " + 
                                    "WHERE Id = @Id";
        
        int rowsAffected = 0;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(updateString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command.Parameters.AddWithValue("@BatteryLevel", device.BatteryLevel);
            
            connection.Open();
            rowsAffected = command.ExecuteNonQuery();
        }
        
        return rowsAffected != -1;
    }
    
    public bool AddPersonalComputer(PersonalComputer device)
    {
        const string insertString =
        "INSERT INTO PersonalComputers (Id, Name, IsTurnedOn, OperatingSystem) " +
        "VALUES (@Id, @Name, @IsTurnedOn, @OperatingSystem)";

        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString)) 
        {
            var command = new SqlCommand(insertString, connection);
            command.Parameters.AddWithValue("@Id", device.Id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command.Parameters.AddWithValue("@OperatingSystem", device.OperationSystem);

        connection.Open();
        rowsAffected = command.ExecuteNonQuery();
        }
        return rowsAffected != -1;
    }

    public bool ModifyPersonalComputer(string id, PersonalComputer device)
    {   
        const string updateString =
            "UPDATE PersonalComputers " +
            "SET Name = @Name, IsTurnedOn = @IsTurnedOn, OperatingSystem = @OperatingSystem " +
            "WHERE Id = @Id";

        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(updateString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command.Parameters.AddWithValue("@OperatingSystem", device.OperationSystem);

            connection.Open();
            rowsAffected = command.ExecuteNonQuery();
        }
        return rowsAffected != -1;
    }

    public bool AddEmbeddedDevice(EmbeddedDevice device)
    {
        const string insertString =
            "INSERT INTO EmbeddedDevices (Id, Name, IsTurnedOn, IpAddress, NetworkName) " +
            "VALUES (@Id, @Name, @IsTurnedOn, @IpAddress, @NetworkName)";

    int rowsAffected = 0;
    using (var connection = new SqlConnection(_connectionString))
    {
        var command = new SqlCommand(insertString, connection);
        command.Parameters.AddWithValue("@Id", device.Id);
        command.Parameters.AddWithValue("@Name", device.Name);
        command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
        command.Parameters.AddWithValue("@IpAddress", device.IpAddress);
        command.Parameters.AddWithValue("@NetworkName", device.NetworkName);

        connection.Open();
        rowsAffected = command.ExecuteNonQuery();
    }
    return rowsAffected != -1;
}

    public bool ModifyEmbeddedDevice(string id, EmbeddedDevice device)
    {
        const string updateString =
            "UPDATE EmbeddedDevices " +
            "SET Name = @Name, IsTurnedOn = @IsTurnedOn, IpAddress = @IpAddress, NetworkName = @NetworkName " +
            "WHERE Id = @Id";

        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(updateString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command.Parameters.AddWithValue("@IpAddress", device.IpAddress);
            command.Parameters.AddWithValue("@NetworkName", device.NetworkName);

            connection.Open();
            rowsAffected = command.ExecuteNonQuery();
        }
        return rowsAffected != -1;
    }

    public bool RemoveDevice(string id)
    {
        const string deleteString = "DELETE FROM Devices WHERE Id = @Id";

        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(deleteString, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            rowsAffected = command.ExecuteNonQuery();
        }
        return rowsAffected != -1;
    }
}