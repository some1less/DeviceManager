INSERT INTO Device (Id, Name, IsTurnedOn) VALUES
    ('SW-1', 'Apple Watch Series 5',    1),
    ('SW-2', 'Samsung Galaxy Watch',    0),
    ('PC-1', 'Dell XPS 15',             1),
    ('PC-2', 'MacBook Pro',             0),
    ('ED-1', 'Raspberry Pi 4',          1),
    ('ED-2', 'Arduino Uno',             0);

INSERT INTO Smartwatch (DeviceId, BatteryLevel) VALUES
    ('SW-1', 85),
    ('SW-2', 60);

INSERT INTO PersonalComputer (DeviceId, OperationSystem) VALUES
    ('PC-1', 'Windows 10'),
    ('PC-2', 'macOS Catalina');

INSERT INTO EmbeddedDevice (DeviceId, IpAddress, NetworkName) VALUES
    ('ED-1', '192.168.1.10', 'OfficeNet'),
    ('ED-2', '10.0.0.5',     'HomeWiFi');