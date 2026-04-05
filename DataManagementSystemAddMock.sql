
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

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'meredith.grey@greysloan.com')
BEGIN
    INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
    VALUES ('admin', 'admin@email.com', 'Admin', 'loc', 'admin');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'derek.shepherd@greysloan.com')
BEGIN
    INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
    VALUES ('Derek Shepherd', 'derek.shepherd@greysloan.com', 'User', 'Seattle', 'password123');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'cristina.yang@greysloan.com')
BEGIN
    INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
    VALUES ('Cristina Yang', 'cristina.yang@greysloan.com', 'User', 'Switzerland', 'password123');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'miranda.bailey@greysloan.com')
BEGIN
    INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
    VALUES ('Miranda Bailey', 'miranda.bailey@greysloan.com', 'User', 'Seattle', 'password123');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'george.omalley@greysloan.com')
BEGIN
    INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
    VALUES ('George O''Malley', 'george.omalley@greysloan.com', 'User', 'Seattle', 'password123');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'izzie.stevens@greysloan.com')
BEGIN
    INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
    VALUES ('Izzie Stevens', 'izzie.stevens@greysloan.com', 'User', 'Seattle', 'password123');
END

GO
