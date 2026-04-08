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

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'Email')
BEGIN
    ALTER TABLE Users ADD Email NVARCHAR(100) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'PasswordHash')
BEGIN
    ALTER TABLE Users ADD PasswordHash NVARCHAR(255) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'UQ_Users_Email' AND type = 'UQ')
BEGIN
    ALTER TABLE Users ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);
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

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Devices]') AND name = 'SerialNumber')
BEGIN
    ALTER TABLE Devices ADD SerialNumber NVARCHAR(100) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'UQ_Devices_SerialNumber' AND type = 'UQ')
BEGIN
    ALTER TABLE Devices ADD CONSTRAINT UQ_Devices_SerialNumber UNIQUE (SerialNumber);
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserDevices]') AND type in (N'U'))
BEGIN
    CREATE TABLE UserDevices (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        DeviceId INT NOT NULL,
        CONSTRAINT UQ_UserDevice UNIQUE (UserId, DeviceId),
        CONSTRAINT FK_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Device FOREIGN KEY (DeviceId) REFERENCES Devices(Id) ON DELETE CASCADE
    );
END
GO