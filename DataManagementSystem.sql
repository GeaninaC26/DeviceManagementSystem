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
        Id INT IDENTITY(1,1) ,
        UserId INT NOT NULL,
        DeviceId INT NOT NULL,
        PRIMARY KEY (UserId, DeviceId),
        CONSTRAINT FK_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Device FOREIGN KEY (DeviceId) REFERENCES Devices(Id) ON DELETE CASCADE
    );
END
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    ALTER TABLE Users
    ADD Email NVARCHAR(100) UNIQUE;
END

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    ALTER TABLE Users
    ADD PasswordHash NVARCHAR(255);
END


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    ALTER TABLE Users
    ADD CONSTRAINT UniqueEmail UNIQUE (Email);
END

