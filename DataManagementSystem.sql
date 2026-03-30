IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagementDB')
BEGIN
    CREATE DATABASE DeviceManagementDB;
END
GO

USE DeviceManagementDB;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(100) NOT NULL,
        UserRole NVARCHAR(50) DEFAULT 'user', 
        UserLocation NVARCHAR(100) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Devices]') AND type in (N'U'))
BEGIN
CREATE TABLE Devices (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DeviceName NVARCHAR(100) NOT NULL,
        Manufacturer NVARCHAR(100) NOT NULL,
        DeviceType NVARCHAR(50) NOT NULL,   
        OS NVARCHAR(50) NOT NULL,
        OSVersion NVARCHAR(20),
        Processor NVARCHAR(100),
        RAM NVARCHAR(20),             
        DeviceDescription NVARCHAR(MAX)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserDevices]') AND type in (N'U'))
BEGIN
    CREATE TABLE UserDevices (
        UserId INT NOT NULL,
        DeviceId INT NOT NULL,
        PRIMARY KEY (UserId, DeviceId),
        CONSTRAINT FK_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Device FOREIGN KEY (DeviceId) REFERENCES Devices(Id) ON DELETE CASCADE
    );
END
GO
---------------------------------------------------------
-- 2. POPULATE USERS 
---------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = 'Meredith Grey')
BEGIN
    INSERT INTO Users (UserName, UserRole, UserLocation)
    VALUES ('Meredith Grey', 'user', 'Seattle');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = 'Derek Shepherd')
BEGIN
    INSERT INTO Users (UserName, UserRole, UserLocation)
    VALUES ('Derek Shepherd', 'user', 'Seattle');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = 'Cristina Yang')
BEGIN
    INSERT INTO Users (UserName, UserRole, UserLocation)
    VALUES ('Cristina Yang', 'user', 'Switzerland');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = 'Miranda Bailey')
BEGIN
    INSERT INTO Users (UserName, UserRole, UserLocation)
    VALUES ('Miranda Bailey', 'user', 'Seattle');
END


---------------------------------------------------------
-- 2. POPULATE DEVICES (Medical Grade Tech)
---------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Devices WHERE DeviceName = 'iPhone 17 Pro')
    INSERT INTO Devices (DeviceName, Manufacturer, DeviceType, OS, OSVersion, Processor, RAM, DeviceDescription)
    VALUES ('iPhone 17 Pro', 'Apple', 'phone', 'iOS', '19.0', 'A19 Pro', '12GB', 'Latest apple smartphone');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE DeviceName = 'Samsung Galaxy S24')
    INSERT INTO Devices (DeviceName, Manufacturer, DeviceType, OS, OSVersion, Processor, RAM, DeviceDescription)
    VALUES ('Samsung Galaxy S24', 'Samsung', 'phone', 'Android', '14.0', 'Snapdragon 8 Gen 3', '8GB', 'Standard issue Android device');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE DeviceName = 'MacBook Pro M3')
    INSERT INTO Devices (DeviceName, Manufacturer, DeviceType, OS, OSVersion, Processor, RAM, DeviceDescription)
    VALUES ('MacBook Pro M3', 'Apple', 'laptop', 'macOS', 'Sonoma', 'M3 Max', '32GB', 'High-end workstation for imaging');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE DeviceName = 'Dell XPS 15')
    INSERT INTO Devices (DeviceName, Manufacturer, DeviceType, OS, OSVersion, Processor, RAM, DeviceDescription)
    VALUES ('Dell XPS 15', 'Dell', 'laptop', 'Windows', '11', 'Intel i9', '16GB', 'Standard administrative laptop');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE DeviceName = 'iPad Pro 12.9')
    INSERT INTO Devices (DeviceName, Manufacturer, DeviceType, OS, OSVersion, Processor, RAM, DeviceDescription)
    VALUES ('iPad Pro 12.9', 'Apple', 'tablet', 'iPadOS', '17.5', 'M2', '8GB', 'Tablet for digital charts');

---------------------------------------------------------
-- 3. POPULATE USER-DEVICE MAPPINGS
---------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE UserName = 'Meredith Grey') AND DeviceId = (SELECT Id FROM Devices WHERE DeviceName = 'MacBook Pro M3'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE UserName = 'Meredith Grey'), (SELECT Id FROM Devices WHERE DeviceName = 'MacBook Pro M3'));

IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE UserName = 'Derek Shepherd') AND DeviceId = (SELECT Id FROM Devices WHERE DeviceName = 'iPhone 17 Pro'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE UserName = 'Derek Shepherd'), (SELECT Id FROM Devices WHERE DeviceName = 'iPhone 17 Pro'));

IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE UserName = 'Cristina Yang') AND DeviceId = (SELECT Id FROM Devices WHERE DeviceName = 'Dell XPS 15'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE UserName = 'Cristina Yang'), (SELECT Id FROM Devices WHERE DeviceName = 'Dell XPS 15'));

IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE UserName = 'Miranda Bailey') AND DeviceId = (SELECT Id FROM Devices WHERE DeviceName = 'iPad Pro 12.9'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE UserName = 'Miranda Bailey'), (SELECT Id FROM Devices WHERE DeviceName = 'iPad Pro 12.9'));

GO

