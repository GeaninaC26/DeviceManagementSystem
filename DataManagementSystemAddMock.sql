USE DeviceManagementDB;
GO

WITH NewUsers (Name, Email, Role, Location) AS (
    SELECT 'Meredith Grey', 'm.grey@email.com', 'User', 'Seattle' UNION ALL
    SELECT 'Derek Shepherd', 'd.shepherd@email.com', 'User', 'Seattle' UNION ALL
    SELECT 'Cristina Yang', 'c.yang@email.com', 'User', 'Switzerland' UNION ALL
    SELECT 'Alex Karev', 'a.karev@email.com', 'User', 'Kansas' UNION ALL
    SELECT 'Miranda Bailey', 'm.bailey@email.com', 'User', 'Seattle' UNION ALL
    SELECT 'Richard Webber', 'r.webber@email.com', 'User', 'Seattle' UNION ALL
    SELECT 'Callie Torres', 'c.torres@email.com', 'User', 'New York' UNION ALL
    SELECT 'Arizona Robbins', 'a.robbins@email.com', 'User', 'New York' UNION ALL
    SELECT 'Jackson Avery', 'j.avery@email.com', 'User', 'Boston' UNION ALL
    SELECT 'April Kepner', 'a.kepner@email.com', 'User', 'Seattle'
)
INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
SELECT nu.Name, nu.Email, nu.Role, nu.Location, 'defaultpasswordhash'
FROM NewUsers nu
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Users.Email = nu.Email);

INSERT INTO Users (Name, Email, Role, Location, PasswordHash)
SELECT 'Admin User', 'admin@email.com', 'Admin', 'Seattle', 'admin'
WHERE NOT EXISTS (
    SELECT 1 FROM Users WHERE Email = 'admin@email.com'
);

WITH NewDevices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description, SerialNumber) AS (
    SELECT 'MacBook Air M3', 'Apple', 'Laptop', 'macOS', 'Sonoma', 'M3', '16GB', 'Ultra-thin laptop', 'MB-AIR-M3-001' UNION ALL
    SELECT 'ThinkPad X1 Carbon', 'Lenovo', 'Laptop', 'Windows', '11 Pro', 'Intel i7', '32GB', 'Business flagship', 'TP-X1-CARBON-001' UNION ALL
    SELECT 'Surface Laptop 5', 'Microsoft', 'Laptop', 'Windows', '11', 'Intel i5', '8GB', 'Touchscreen laptop', 'SURFACE-LAPTOP-5-001' UNION ALL
    SELECT 'Dell XPS 13', 'Dell', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', 'InfinityEdge display', 'DELL-XPS-13-001' UNION ALL
    SELECT 'HP Spectre x360', 'HP', 'Laptop', 'Windows', '11', 'Intel i7', '16GB', '2-in-1 convertible', 'HP-SPECTRE-X360-001' UNION ALL
    SELECT 'iPad Pro 12.9', 'Apple', 'Tablet', 'iPadOS', '17', 'M2', '8GB', 'High-end tablet', 'IPAD-PRO-12.9-001' UNION ALL
    SELECT 'Galaxy Tab S9', 'Samsung', 'Tablet', 'Android', '14', 'Snapdragon G2', '12GB', 'AMOLED tablet', 'GALAXY-TAB-S9-001' UNION ALL
    SELECT 'Surface Pro 9', 'Microsoft', 'Tablet', 'Windows', '11', 'Intel i5', '16GB', 'Tablet that replaces laptop', 'SURFACE-PRO-9-001' UNION ALL
    SELECT 'iPhone 15 Pro', 'Apple', 'Phone', 'iOS', '17', 'A17 Pro', '8GB', 'Titanium build', 'IPHONE-15-PRO-001' UNION ALL
    SELECT 'Galaxy S24 Ultra', 'Samsung', 'Phone', 'Android', '14', 'Snapdragon G3', '12GB', 'AI integrated phone', 'GALAXY-S24-ULTRA-001' UNION ALL
    SELECT 'Pixel 8 Pro', 'Google', 'Phone', 'Android', '14', 'Tensor G3', '12GB', 'Pure Android experience', 'PIXEL-8-PRO-001' UNION ALL
    SELECT 'iPhone SE', 'Apple', 'Phone', 'iOS', '16', 'A15', '4GB', 'Budget company phone', 'IPHONE-SE-001' UNION ALL
    SELECT 'Apple Watch Series 9', 'Apple', 'Watch', 'watchOS', '10', 'S9', '1GB', 'Health tracking', 'APPLE-WATCH-SERIES-9-001' UNION ALL
    SELECT 'Galaxy Watch 6', 'Samsung', 'Watch', 'WearOS', '4', 'Exynos W930', '2GB', 'Round smartwatch', 'GALAXY-WATCH-6-001' UNION ALL
    SELECT 'Pixel Watch 2', 'Google', 'Watch', 'WearOS', '4', 'SW5100', '2GB', 'Fitbit integration', 'PIXEL-WATCH-2-001'
)
INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description, SerialNumber)
SELECT nd.Name, nd.Manufacturer, nd.Type, nd.OS, nd.OSVersion, nd.Processor, nd.RAM, nd.Description, nd.SerialNumber
FROM NewDevices nd
WHERE NOT EXISTS (SELECT 1 FROM Devices WHERE Devices.SerialNumber = nd.SerialNumber);


INSERT INTO UserDevices (UserId, DeviceId)
SELECT u.Id, d.Id FROM Users u, Devices d 
WHERE u.Email = 'm.grey@email.com' AND d.Name = 'MacBook Air M3'
AND NOT EXISTS (SELECT 1 FROM UserDevices ud WHERE ud.UserId = u.Id AND ud.DeviceId = d.Id);

INSERT INTO UserDevices (UserId, DeviceId)
SELECT u.Id, d.Id FROM Users u, Devices d 
WHERE u.Email = 'c.yang@email.com' AND d.Name = 'ThinkPad X1 Carbon'
AND NOT EXISTS (SELECT 1 FROM UserDevices ud WHERE ud.UserId = u.Id AND ud.DeviceId = d.Id);

INSERT INTO UserDevices (UserId, DeviceId)
SELECT u.Id, d.Id FROM Users u, Devices d 
WHERE u.Email = 'd.shepherd@email.com' AND d.Name = 'iPhone 15 Pro'
AND NOT EXISTS (SELECT 1 FROM UserDevices ud WHERE ud.UserId = u.Id AND ud.DeviceId = d.Id);
GO