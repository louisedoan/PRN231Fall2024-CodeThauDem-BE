-- Insert data into Membership table
INSERT INTO Membership (Rank, Discount) VALUES 
('Bronze', 0),
('Silver', 3),
('Gold', 5);

-- Insert data into User table
INSERT INTO [User] (Email, Password, Gender, Nationality, Address, Fullname, DOB, Role, MembershipID, Status) VALUES 
('c1@gmail.com', '123456', 'Female', 'American', '123 Elm St', 'Jane Doe', '1990-01-01', 'Member', 1, 'Active'),
('c2@gmail.com', '123456', 'Male', 'British', '456 Maple Ave', 'John Smith', '1985-05-15', 'Member', 1, 'Active'),
('admin@gmail.com', '123456', 'Male', 'Australian', '321 Pine Rd', 'Bob Brown', '1980-12-11', 'Admin', NULL, 'Active');

-- Insert data into Plane table
INSERT INTO Plane (PlaneCode, TotalSeats, Status) VALUES 
('Plane001', 42, 'InUse'),
('Plane002', 42, 'InUse'),
('Plane003', 42, 'InUse'),
('Plane004', 42, 'Available'),
('Plane005', 42, 'Available'),
('Plane006', 42, 'Available'),
('Plane007', 42, 'Available'),
('Plane008', 42, 'Available'),
('Plane009', 42, 'Available'),
('Plane010', 42, 'Available');
-- Insert data into FlightRoute table
INSERT INTO FlightRoute (Location) VALUES 
('SGN'),
('HANOI'),
('LONGTHANH'),
('DANANG'),
('NHATRANG'),
('PHUYEN'),
('QUANGNINH'),
('HUE');
-- Insert data into Flight table for flights on 25/10
INSERT INTO Flight (FlightNumber, PlaneID, DepartureLocation, DepartureTime, ArrivalLocation, ArrivalTime, FlightStatus) VALUES
(101, 1, 1, '2024-11-15 08:00:00', 2, '2024-11-15 18:00:00', 'Available'),
(102, 2, 2, '2024-11-15 09:00:00', 3, '2024-11-15 23:00:00', 'Available'),
(103, 3, 2, '2024-11-16 10:00:00', 1, '2024-11-16 20:00:00', 'Available');

-- Insert Seats for each Plane from 1 to 10
DECLARE @planeId INT = 1;

WHILE @planeId <= 10
BEGIN
    DECLARE @i INT = 1;
    WHILE @i <= 42
    BEGIN
        INSERT INTO Seat (PlaneID, SeatNumer, Class, Status, Price) VALUES
        (@planeId, @i, CASE WHEN @i <= 12 THEN 'Business' ELSE 'Economy' END, 'Available', CASE WHEN @i <= 12 THEN 2000000 ELSE 1500000 END);
        SET @i = @i + 1;
    END
    SET @planeId = @planeId + 1;
END
