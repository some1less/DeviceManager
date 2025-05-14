using System.Data;
using DTOs;
using Microsoft.Data.SqlClient;
using Models;

namespace Infrastructure;

public class DeviceRepository : IDeviceRepository
{
    
    private readonly string _connectionString;

    public DeviceRepository(string connectionString)
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

    public Device? GetDeviceById(string id)
    {
        if (id.StartsWith("SW-"))
        {
            const string sql = @"SELECT d.Id, d.Name, d.IsTurnedOn, s.BatteryLevel, d.DeviceRowVersion, s.RowVersion
                               FROM Smartwatch s INNER JOIN Device d ON s.DeviceId = d.Id  
                                   WHERE s.DeviceId = @deviceId";
            
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
                            BatteryLevel = reader.GetInt32(3),
                            OriginalVersion = reader.GetSqlBinary(reader.GetOrdinal("DeviceRowVersion")).Value,
                            SwRowVersion = reader.GetSqlBinary(reader.GetOrdinal("RowVersion")).Value
                        };
                    }
                }
                
                throw new InvalidOperationException($"Smartwatch is not found: '{id}'");
            }
            
        }

        if (id.StartsWith("PC-"))
        {
            const string sql = "select d.Id, d.Name, d.IsTurnedOn, pc.OperationSystem, d.DeviceRowVersion, pc.RowVersion " + 
                               "from PersonalComputer pc INNER JOIN Device d ON pc.DeviceID = d.Id " +
                               "where pc.DeviceId = @deviceId";
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
                            OperationSystem = reader.GetString(3),
                            OriginalVersion = reader.GetSqlBinary(reader.GetOrdinal("DeviceRowVersion")).Value,
                            PcRowVersion = reader.GetSqlBinary(reader.GetOrdinal("RowVersion")).Value
                            
                        };
                    }
                }

                throw new InvalidOperationException($"Personal Computer is not found: '{id}'");
            }
        }

        if (id.StartsWith("ED-"))
        {
            const string sql = "select d.Id, d.Name, d.IsTurnedOn, ed.IpAddress, ed.NetworkName, d.DeviceRowVersion, ed.RowVersion " +
                               "from EmbeddedDevice ed INNER JOIN Device d ON ed.DeviceID = d.Id " +
                               "where ed.DeviceId = @deviceId";
            
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
                            NetworkName = reader.GetString(4),
                            OriginalVersion = reader.GetSqlBinary(reader.GetOrdinal("DeviceRowVersion")).Value,
                            EdRowVersion = reader.GetSqlBinary(reader.GetOrdinal("RowVersion")).Value
                            
                        };
                    }
                }

                throw new InvalidOperationException($"Embedded Device is not found: '{id}'");
            }
        }
        throw new InvalidOperationException($"Device is not found: '{id}'");
    }

    public void AddSmartwatch(Smartwatch device)
    {
        /*it was said that user can't provide id
        so while creating user will specify name, ...  and DEVICE TYPE
        finally in deserialization it will found addSmartwatch option - OK
        so here I have to make only automatic ID creation
        
        count how many smartwatches already existing
        udp: now to avoid error while creating new sw after deleting sw4
        maxSql implemented */
        
        const string maxSql =
            "SELECT MAX(CONVERT(INT, SUBSTRING(Id, 4, Len(Id)-3))) " + 
            "FROM Device " +
            "WHERE Id like 'SW-%'";
        
        /*{
            "Name": "hello",
            "IsTurnedOn": true,
            "deviceType": "smartwatch",
            "BatteryLevel": 80
        }*/
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(maxSql, connection);
            int maxVal = (int)command.ExecuteScalar()!;
            var newId = $"SW-{ maxVal + 1 }";
            device.Id = newId;
            
            SqlCommand command2 = new SqlCommand("AddSmartwatch", connection);
            
            Console.WriteLine($"Calling {command2.CommandText} as {command2.CommandType}");
            Console.WriteLine($"Connected to DB: {connection.Database}");
            command2.CommandType = CommandType.StoredProcedure;
            
            command2.Parameters.AddWithValue("@DeviceId", device.Id);
            command2.Parameters.AddWithValue("@Name", device.Name);
            command2.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command2.Parameters.AddWithValue("@BatteryLevel", device.BatteryLevel);
            
            command2.ExecuteNonQuery();
            
        }
    }
    
    public void AddPersonalComputer(PersonalComputer device)
    {
        const string maxSql =
            "SELECT MAX(CONVERT(INT, SUBSTRING(Id, 4, Len(Id)-3))) " + 
            "FROM Device " +
            "WHERE Id like 'PC-%'";
        
        
        using (var connection = new SqlConnection(_connectionString)) 
        {
            connection.Open();
            
            SqlCommand command = new SqlCommand(maxSql, connection);
            int maxVal = (int)command.ExecuteScalar()!;
            
            var newId = $"PC-{ maxVal + 1 }";
            device.Id = newId;
            
            SqlCommand command2 = new SqlCommand("AddPersonalComputer", connection);
            command2.CommandType = CommandType.StoredProcedure;
            command2.Parameters.AddWithValue("@DeviceId", device.Id);
            command2.Parameters.AddWithValue("@Name", device.Name);
            command2.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command2.Parameters.AddWithValue("@OperationSystem", device.OperationSystem);
            
            command2.ExecuteNonQuery();
        }
    }
    
    public void AddEmbeddedDevice(EmbeddedDevice device)
    {
        const string maxSql =
            "SELECT MAX(CONVERT(INT, SUBSTRING(Id, 4, Len(Id)-3))) " + 
            "FROM Device " +
            "WHERE Id like 'ED-%'";

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
                
            SqlCommand command = new SqlCommand(maxSql, connection);
            var count = (int)command.ExecuteScalar()!;
                
            var newId = $"ED-{ count + 1 }";
            device.Id = newId;
            
            SqlCommand command2 = new SqlCommand("AddEmbedded", connection);
            command2.CommandType = CommandType.StoredProcedure;
            command2.Parameters.AddWithValue("@DeviceId", device.Id);
            command2.Parameters.AddWithValue("@Name", device.Name);
            command2.Parameters.AddWithValue("@IsTurnedOn", device.IsTurnedOn);
            command2.Parameters.AddWithValue("@IpAddress", device.IpAddress);
            command2.Parameters.AddWithValue("@NetworkName", device.NetworkName);

            command2.ExecuteNonQuery();
        }
    }

    public async Task<bool> ModifySmartwatch(Smartwatch smartwatch)
    {
        // I should separate sqls to device and sw
        
        int rowsAffected = 0;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            // start transact
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {

                byte[] deviceVersion = null;
                byte[] smartwatchVersion = null;

                var rowVersionQuery = @"SELECT d.DeviceRowVersion AS deviceVersion, sw.RowVersion AS smartwatchVersion
                            FROM Device d
                            INNER JOIN Smartwatch sw ON d.ID = sw.DeviceId
                            WHERE d.Id = @Id";

                using (var rowVersionCommand = new SqlCommand(rowVersionQuery, connection, transaction))
                {
                    rowVersionCommand.Parameters.AddWithValue("@Id", smartwatch.Id);
                    using (var reader = rowVersionCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deviceVersion = (byte[])reader["deviceVersion"];
                            smartwatchVersion = (byte[])reader["smartwatchVersion"];
                        }
                        else
                        {
                            throw new InvalidOperationException($"Smartwatch doesn't exist: {smartwatch.Id}");
                        }
                    }
                }
                const string deviceSql = "" +
                                         "UPDATE Device " +
                                         "SET Name = @Name, IsTurnedOn = @IsTurnedOn " +
                                         "WHERE Id = @Id AND DeviceRowVersion = @deviceVersion";
        
                const string updateString = "UPDATE Smartwatch " + 
                                            "SET BatteryLevel = @BatteryLevel " + 
                                            "WHERE DeviceId = @Id AND RowVersion = @smartwatchVersion";
                
                using (SqlCommand updateDeviceCommand = new SqlCommand(deviceSql, connection, transaction))
                {
                    updateDeviceCommand.Parameters.AddWithValue("@Id", smartwatch.Id);
                    updateDeviceCommand.Parameters.AddWithValue("@Name", smartwatch.Name);
                    updateDeviceCommand.Parameters.AddWithValue("@IsTurnedOn", smartwatch.IsTurnedOn);
                    updateDeviceCommand.Parameters.Add("@deviceVersion", SqlDbType.Timestamp).Value = deviceVersion;

                    rowsAffected = await updateDeviceCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                using (SqlCommand updateSwCommand = new SqlCommand(updateString, connection, transaction))
                {
                    updateSwCommand.Parameters.AddWithValue("@Id", smartwatch.Id);
                    updateSwCommand.Parameters.AddWithValue("@BatteryLevel", smartwatch.BatteryLevel);
                    updateSwCommand.Parameters.Add("@smartwatchVersion", SqlDbType.Timestamp).Value = smartwatchVersion;
                    
                    rowsAffected = await updateSwCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            
            
        }
    }
    

    public async Task<bool> ModifyPersonalComputer(PersonalComputer personalComputer)
    {
        int rowsAffected = 0;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                
                byte[] deviceVersion = null;
                byte[] pcVersion = null;

                var rowVersionQuery = @"SELECT d.DeviceRowVersion AS deviceVersion, pc.RowVersion AS personalComputerVersion
                            FROM Device d
                            INNER JOIN PersonalComputer pc ON d.ID = pc.DeviceId
                            WHERE d.Id = @Id";

                using (var rowVersionCommand = new SqlCommand(rowVersionQuery, connection, transaction))
                {
                    rowVersionCommand.Parameters.AddWithValue("@Id", personalComputer.Id);
                    using (var reader = rowVersionCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deviceVersion = (byte[])reader["deviceVersion"];
                            pcVersion = (byte[])reader["personalComputerVersion"];
                        }
                        else
                        {
                            throw new InvalidOperationException($"Personal Computer doesn't exist: {personalComputer.Id}");
                        }
                    }
                }
                
                const string deviceSql = 
                    "UPDATE Device " +
                    "SET Name = @Name, IsTurnedOn = @IsTurnedOn " +
                    "WHERE Id = @Id AND DeviceRowVersion = @deviceVersion";
         
                const string pcSql =
                    "UPDATE PersonalComputer " +
                    "SET OperationSystem = @OperationSystem " +
                    "WHERE DeviceId = @Id AND RowVersion = @personalComputerVersion";
                
                using (SqlCommand command = new SqlCommand(deviceSql, connection, transaction))
                {

                    command.Parameters.AddWithValue("@Id", personalComputer.Id);
                    command.Parameters.AddWithValue("@Name", personalComputer.Name);
                    command.Parameters.AddWithValue("@IsTurnedOn", personalComputer.IsTurnedOn);
                    command.Parameters.Add("@deviceVersion", SqlDbType.Timestamp).Value = deviceVersion;


                    rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                using (SqlCommand command1 = new SqlCommand(pcSql, connection, transaction))
                {
                    command1.Parameters.AddWithValue("@Id", personalComputer.Id);
                    command1.Parameters.AddWithValue("@OperationSystem", personalComputer.OperationSystem);
                    command1.Parameters.Add("@personalComputerVersion", SqlDbType.Timestamp).Value = pcVersion;
                        
                    rowsAffected = await command1.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                transaction.Commit();
                return true;
                
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            
        }
    }

    public async Task<bool> ModifyEmbeddedDevice(EmbeddedDevice embeddedDevice)
    {
        int rowsAffected = 0;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {

                byte[] deviceVersion = null;
                byte[] edVersion = null;

                var rowVersionQuery =
                    @"SELECT d.DeviceRowVersion AS deviceVersion, ed.RowVersion AS embeddedDeviceVersion
                            FROM Device d
                            INNER JOIN EmbeddedDevice ed ON d.ID = ed.DeviceId
                            WHERE d.Id = @Id";

                using (var rowVersionCommand = new SqlCommand(rowVersionQuery, connection, transaction))
                {
                    rowVersionCommand.Parameters.AddWithValue("@Id", embeddedDevice.Id);
                    using (var reader = rowVersionCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deviceVersion = (byte[])reader["deviceVersion"];
                            edVersion = (byte[])reader["embeddedDeviceVersion"];
                        }
                        else
                        {
                            throw new InvalidOperationException($"Embedded Device doesn't exist: {embeddedDevice.Id}");
                        }
                    }
                }
                
                const string deviceSql =
                    @"UPDATE Device 
                    SET Name = @Name, IsTurnedOn = @IsTurnedOn 
                    WHERE Id = @Id AND DeviceRowVersion = @deviceVersion";
         
                const string edSql =
                    @"UPDATE EmbeddedDevice
                    SET IpAddress = @IpAddress, NetworkName = @NetworkName
                    WHERE DeviceId = @Id AND RowVersion = @embeddedDeviceVersion";

                using (SqlCommand command = new SqlCommand(deviceSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@Id", embeddedDevice.Id);
                    command.Parameters.AddWithValue("@Name", embeddedDevice.Name);
                    command.Parameters.AddWithValue("@IsTurnedOn", embeddedDevice.IsTurnedOn);
                    command.Parameters.Add("@deviceVersion", SqlDbType.Timestamp).Value = deviceVersion;

                    rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                using (SqlCommand command1 = new SqlCommand(edSql, connection, transaction))
                {
                    command1.Parameters.AddWithValue("@Id", embeddedDevice.Id);
                    command1.Parameters.AddWithValue("@IpAddress", embeddedDevice.IpAddress);
                    command1.Parameters.AddWithValue("@NetworkName", embeddedDevice.NetworkName);
                    command1.Parameters.Add("@embeddedDeviceVersion", SqlDbType.Timestamp).Value = edVersion;


                    rowsAffected = await command1.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task<bool> RemoveDevice(string id)
    {
        string kidSql;
        if (id.StartsWith("SW-"))
        {
            kidSql = "DELETE FROM Smartwatch WHERE DeviceID = @Id";
        } else if (id.StartsWith("PC-"))
        {
            kidSql = "DELETE FROM PersonalComputer WHERE DeviceId = @Id";
        }
        else
        {
            kidSql = "DELETE FROM EmbeddedDevice WHERE DeviceID = @Id";
        }

        const string deviceSql = "DELETE FROM Device WHERE Id = @Id"; 

        
        int rowsAffected = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using (var cmdChild = new SqlCommand(kidSql, connection, transaction))
                {
                    cmdChild.Parameters.AddWithValue("@Id", id);
                    
                    rowsAffected = await cmdChild.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                using (var cmdDev = new SqlCommand(deviceSql, connection, transaction))
                {
                    cmdDev.Parameters.AddWithValue("@Id", id);

                    rowsAffected = await cmdDev.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }
    }
    
}