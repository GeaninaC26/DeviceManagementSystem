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
        Name NVARCHAR(100) NOT NULL,
        Role NVARCHAR(50) DEFAULT 'User', 
        Location NVARCHAR(100) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Devices]') AND type in (N'U'))
BEGIN
CREATE TABLE Devices (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Manufacturer NVARCHAR(100) NOT NULL,
        Type NVARCHAR(50) NOT NULL,   
        OS NVARCHAR(50) NOT NULL,
        OSVersion NVARCHAR(20),
        Processor NVARCHAR(100),
        RAM NVARCHAR(20),             
        Description NVARCHAR(MAX)
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

IF NOT EXISTS (SELECT 1 FROM Users WHERE Name = 'Meredith Grey')
BEGIN
    INSERT INTO Users (Name, Role, Location)
    VALUES ('Meredith Grey', 'User', 'Seattle');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Name = 'Derek Shepherd')
BEGIN
    INSERT INTO Users (Name, Role, Location)
    VALUES ('Derek Shepherd', 'User', 'Seattle');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Name = 'Cristina Yang')
BEGIN
    INSERT INTO Users (Name, Role, Location)
    VALUES ('Cristina Yang', 'User', 'Switzerland');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Name = 'Miranda Bailey')
BEGIN
    INSERT INTO Users (Name, Role, Location)
    VALUES ('Miranda Bailey', 'User', 'Seattle');
END


---------------------------------------------------------
-- 2. POPULATE DEVICES (Medical Grade Tech)
---------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'iPhone 17 Pro')
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description)
    VALUES ('iPhone 17 Pro', 'Apple', 'phone', 'iOS', '19.0', 'A19 Pro', '12GB', 'Latest apple smartphone');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'Samsung Galaxy S24')
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description)
    VALUES ('Samsung Galaxy S24', 'Samsung', 'phone', 'Android', '14.0', 'Snapdragon 8 Gen 3', '8GB', 'Standard issue Android device');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'MacBook Pro M3')
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description)
    VALUES ('MacBook Pro M3', 'Apple', 'laptop', 'macOS', 'Sonoma', 'M3 Max', '32GB', 'High-end workstation for imaging');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'Dell XPS 15')
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description)
    VALUES ('Dell XPS 15', 'Dell', 'laptop', 'Windows', '11', 'Intel i9', '16GB', 'Standard administrative laptop');

IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'iPad Pro 12.9')
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description)
    VALUES ('iPad Pro 12.9', 'Apple', 'tablet', 'iPadOS', '17.5', 'M2', '8GB', 'Tablet for digital charts');

---------------------------------------------------------
-- 3. POPULATE USER-DEVICE MAPPINGS
---------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE Name = 'Meredith Grey') AND DeviceId = (SELECT Id FROM Devices WHERE Name = 'MacBook Pro M3'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE Name = 'Meredith Grey'), (SELECT Id FROM Devices WHERE Name = 'MacBook Pro M3'));

IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE Name = 'Derek Shepherd') AND DeviceId = (SELECT Id FROM Devices WHERE Name = 'iPhone 17 Pro'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE Name = 'Derek Shepherd'), (SELECT Id FROM Devices WHERE Name = 'iPhone 17 Pro'));

IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE Name = 'Cristina Yang') AND DeviceId = (SELECT Id FROM Devices WHERE Name = 'Dell XPS 15'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE Name = 'Cristina Yang'), (SELECT Id FROM Devices WHERE Name = 'Dell XPS 15'));

IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE UserId = (SELECT Id FROM Users WHERE Name = 'Miranda Bailey') AND DeviceId = (SELECT Id FROM Devices WHERE Name = 'iPad Pro 12.9'))
    INSERT INTO UserDevices (UserId, DeviceId) VALUES ((SELECT Id FROM Users WHERE Name = 'Miranda Bailey'), (SELECT Id FROM Devices WHERE Name = 'iPad Pro 12.9'));

GO

