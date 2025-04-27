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
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
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

    public Device GetDeviceById(string id)
    {
        if (id.StartsWith("SW-"))
        {
            const string sql = "SELECT d.Id, d.Name, d.IsTurnedOn, s.BatteryLevel " +
                               "FROM Smartwatch s INNER JOIN Device d ON s.DeviceId = d.Id WHERE s.DeviceId = @deviceId";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DeviceId", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Smartwatch
                        {
                            Id = reader.GetString(0),
                            Name         = reader.GetString(1),
                            IsTurnedOn   = reader.GetBoolean(2),
                            BatteryLevel = reader.GetInt32(3)
                        };
                    }
                }
                
                throw new InvalidOperationException($"Smartwatch is not found: '{id}'");
            }
            
        }

        if (id.StartsWith("PC-"))
        {
            const string sql = "select d.Id, d.Name, d.IsTurnedOn, pc.OperationSystem " + 
                               " from PersonalComputer pc INNER JOIN Device d ON pc.DeviceID = d.Id" +
                               " where pc.DeviceId = @deviceId";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DeviceId", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PersonalComputer
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            IsTurnedOn = reader.GetBoolean(2),
                            OperationSystem = reader.GetString(3)
                        };
                    }
                }

                throw new InvalidOperationException($"Personal Computer is not found: '{id}'");
            }
        }

        if (id.StartsWith("ED-"))
        {
            const string sql = "select d.Id, d.Name, d.IsTurnedOn, ed.IpAddress, ed.NetworkName" + 
                               " from EmbeddedDevice ed INNER JOIN Device d ON ed.DeviceID = d.Id" +
                               " where ed.DeviceId = @deviceId";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@deviceId", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new EmbeddedDevice()
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            IsTurnedOn = reader.GetBoolean(2),
                            IpAddress = reader.GetString(3),
                            NetworkName = reader.GetString(4)
                        };
                    }
                }

                throw new InvalidOperationException($"Embedded Device is not found: '{id}'");
            }
        }
        throw new InvalidOperationException($"Device is not found: '{id}'");
    }
    
    public bool AddSmartwatch(Smartwatch device)
    {
        // it was said that user can't provide id
        // so while creating user will specify name, ...  and DEVICE TYPE
        // finally in deserialization it will found addSmartwatch option - OK
        // so here I have to make only automatic ID creation
        
        // count how many smartwatches already existing
        const string countSql =
            "SELECT COUNT(*) FROM Smartwatch";
        
        const string insertDevice =
            "INSERT INTO Device (Id, Name, IsTurnedOn) VALUES (@Id, @name, @isTurnedOn)";
        const string insertSw =
            "INSERT INTO Smartwatch (DeviceId, BatteryLevel) VALUES (@DeviceId, @BatteryLevel)";
        
        /*{
            "Name": "hello",
            "IsTurnedOn": true,
            "deviceType": "smartwatch",
            "BatteryLevel": 80
        }*/
        
        int rowsAffected = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            
            SqlCommand command = new SqlCommand(countSql, connection);
            int count = (int)command.ExecuteScalar()!;
            
            var newId = $"SW-{ count + 1 }";
            device.Id = newId;
            
            SqlCommand command1 = new SqlCommand(insertDevice, connection);
            command1.Parameters.AddWithValue("@Id", device.Id);
            command1.Parameters.AddWithValue("@Name", device.Name);
            command1.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            rowsAffected += command1.ExecuteNonQuery();
            
            SqlCommand command2 = new SqlCommand(insertSw, connection);
            command2.Parameters.AddWithValue("@DeviceId", device.Id);
            command2.Parameters.AddWithValue("@BatteryLevel", device.BatteryLevel);
            rowsAffected += command2.ExecuteNonQuery();
        }
        
        return rowsAffected != -1;

    }
    public bool ModifySmartwatch(string id, Smartwatch device)
    {
        // I should separate sqls to device and sw
        
        const string deviceSql = "" +
                                 "UPDATE Device " +
                                 "SET Name = @Name, IsTurnedOn = @IsTurnedOn " +
                                 "WHERE Id = @Id";
        
        const string updateString = "UPDATE Smartwatch " + 
                                    "SET BatteryLevel = @BatteryLevel " + 
                                    "WHERE DeviceId = @Id";
        
        int rowsAffected = 0;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            
            SqlCommand command = new SqlCommand(deviceSql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            rowsAffected += command.ExecuteNonQuery();

            
            SqlCommand command1 = new SqlCommand(updateString, connection);
            command1.Parameters.AddWithValue("@Id", id);
            command1.Parameters.AddWithValue("@BatteryLevel", device.BatteryLevel);
            rowsAffected += command1.ExecuteNonQuery();
            
        }
        
        return rowsAffected != -1;
    }
    
     public bool AddPersonalComputer(PersonalComputer device)
     {
         const string countSql =
             "SELECT COUNT(*) FROM Smartwatch";
         
         const string deviceSql =
         "INSERT INTO Device (Id, Name, IsTurnedOn) VALUES (@Id, @Name, @IsTurnedOn)";
         
         const string pcSql =
             "INSERT INTO PersonalComputer (DeviceId, OperationSystem) VALUES (@DeviceId, @OperationSystem)";

         int rowsAffected = 0;
         using (var connection = new SqlConnection(_connectionString)) 
         {
             connection.Open();
            
             SqlCommand command = new SqlCommand(countSql, connection);
             int count = (int)command.ExecuteScalar()!;
            
             var newId = $"PC-{ count + 1 }";
             device.Id = newId;
            
             SqlCommand command1 = new SqlCommand(deviceSql, connection);
             command1.Parameters.AddWithValue("@Id", device.Id);
             command1.Parameters.AddWithValue("@Name", device.Name);
             command1.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
             rowsAffected += command1.ExecuteNonQuery();
            
             SqlCommand command2 = new SqlCommand(pcSql, connection);
             command2.Parameters.AddWithValue("@DeviceId", device.Id);
             command2.Parameters.AddWithValue("@OperationSystem", device.OperationSystem);
             rowsAffected += command2.ExecuteNonQuery();
         }
         return rowsAffected != -1;
     }

     public bool ModifyPersonalComputer(string id, PersonalComputer device)
     {   
         const string deviceSql = 
             "UPDATE Device " +
             "SET Name = @Name, IsTurnedOn = @IsTurnedOn " +
             "WHERE Id = @Id";
         
         const string pcSql =
             "UPDATE PersonalComputer " +
             "SET OperationSystem = @OperationSystem " +
             "WHERE DeviceId = @Id";

         int rowsAffected = 0;
         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
            
             SqlCommand command = new SqlCommand(deviceSql, connection);
             command.Parameters.AddWithValue("@Id", id);
             command.Parameters.AddWithValue("@Name", device.Name);
             command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
             rowsAffected += command.ExecuteNonQuery();

            
             SqlCommand command1 = new SqlCommand(pcSql, connection);
             command1.Parameters.AddWithValue("@Id", id);
             command1.Parameters.AddWithValue("@OperationSystem", device.OperationSystem);
             rowsAffected += command1.ExecuteNonQuery();
         }
         return rowsAffected != -1;
     }

    public bool AddEmbeddedDevice(EmbeddedDevice device)
    {
        
        const string countSql =
            "SELECT COUNT(*) FROM EmbeddedDevice";
        
        const string deviceSql =
            "INSERT INTO Device (Id, Name, IsTurnedOn) VALUES (@Id, @Name, @IsTurnedOn)";
        
        const string edSql =
            "INSERT INTO EmbeddedDevice (DeviceId, IpAddress, NetworkName) " +
            "VALUES (@DeviceId, @IpAddress, @NetworkName)";

        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
                
            SqlCommand command = new SqlCommand(countSql, connection);
            int count = (int)command.ExecuteScalar()!;
                
            var newId = $"ED-{ count + 1 }";
            device.Id = newId;
                
            SqlCommand command1 = new SqlCommand(deviceSql, connection);
            command1.Parameters.AddWithValue("@Id", device.Id);
            command1.Parameters.AddWithValue("@Name", device.Name);
            command1.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            rowsAffected += command1.ExecuteNonQuery();
                
            SqlCommand command2 = new SqlCommand(edSql, connection);
            command2.Parameters.AddWithValue("@DeviceId", device.Id);
            command2.Parameters.AddWithValue("@IpAddress", device.IpAddress);
            command2.Parameters.AddWithValue("@NetworkName", device.NetworkName);
            rowsAffected += command2.ExecuteNonQuery();
        }
        return rowsAffected != -1; 
    }

     public bool ModifyEmbeddedDevice(string id, EmbeddedDevice device)
     {
         
         const string deviceSql =
             "UPDATE Device " +
             "SET Name = @Name, IsTurnedOn = @IsTurnedOn " +
             "WHERE Id = @Id";
         
         const string edSql =
             "UPDATE EmbeddedDevice " +
             "SET IpAddress = @IpAddress, NetworkName = @NetworkName " +
             "WHERE DeviceId = @Id";

         int rowsAffected = 0;
         using (var connection = new SqlConnection(_connectionString))
         {
             connection.Open();
            
             SqlCommand command = new SqlCommand(deviceSql, connection);
             command.Parameters.AddWithValue("@Id", id);
             command.Parameters.AddWithValue("@Name", device.Name);
             command.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
             rowsAffected += command.ExecuteNonQuery();

            
             SqlCommand command1 = new SqlCommand(edSql, connection);
             command1.Parameters.AddWithValue("@Id", id);
             command1.Parameters.AddWithValue("@IpAddress", device.IpAddress);
             command1.Parameters.AddWithValue("@NetworkName", device.NetworkName);
             rowsAffected += command1.ExecuteNonQuery();
         }
         return rowsAffected != -1;
     }

    public bool RemoveDevice(string id)
    {
        const string deleteString = "DELETE FROM Device WHERE Id = @Id";

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