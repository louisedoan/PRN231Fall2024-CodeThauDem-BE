-- Insert data into Membership table
INSERT INTO Membership (Rank, Discount) VALUES 
('Bronze', 0),
('Silver', 0.03),
('Gold', 0.05);

-- Insert data into User table
INSERT INTO [User] (Email, Password, Gender, Nationality, Address, Fullname, DOB, Role, MembershipID, Status) VALUES 
('c1@gmail.com', '123456', 'Female', 'American', '123 Elm St', 'Jane Doe', '1990-01-01', 'Customer', 1, 'Active'),
('c2@gmail.com', '123456', 'Male', 'British', '456 Maple Ave', 'John Smith', '1985-05-15', 'Customer', 1, 'Active'),
('manager@gmail.com', '123456', 'Female', 'Canadian', '789 Oak Blvd', 'Alice Johnson', '1978-08-23', 'Manager', NULL, 'Active'),
('admin@gmail.com', '123456', 'Male', 'Australian', '321 Pine Rd', 'Bob Brown', '1980-12-11', 'Admin', NULL, 'Active');

-- Insert data into Plane table
INSERT INTO Plane (PlaneCode, TotalSeats, Status) VALUES 
('Plane001', 42, 'Active'),
('Plane002', 42, 'Active');
-- Insert data into FlightRoute table
INSERT INTO FlightRoute (Location) VALUES 
('SGN'),
('HANOI'),
('LONGTHANH');
-- Insert data into Flight table for flights on 25/10
INSERT INTO Flight (FlightNumber, PlaneID, DepartureLocation, DepartureTime, ArrivalLocation, ArrivalTime, FlightStatus) VALUES
(101, 1, 1, '2024-10-25 08:00:00', 2, '2024-10-19 18:00:00', 'Available'),
(102, 2, 2, '2024-10-25 09:00:00', 3, '2024-10-19 23:00:00', 'Available'),
(103, 1, 3, '2024-10-25 10:00:00', 1, '2024-10-19 20:00:00', 'Available');
-- Insert data into Seat table for Plane 1
DECLARE @i INT = 1;
WHILE @i <= 42
BEGIN
    INSERT INTO Seat (PlaneID, SeatNumer, Class, Status, Price) VALUES
    (1, @i, CASE WHEN @i <= 12 THEN 'Business' ELSE 'Economy' END, 'Available', CASE WHEN @i <= 12 THEN 2000000 ELSE 1500000 END);
    SET @i = @i + 1;
END

-- Insert data into Seat table for Plane 2
SET @i = 1;
WHILE @i <= 42
BEGIN
    INSERT INTO Seat (PlaneID, SeatNumer, Class, Status, Price) VALUES
    (2, @i, CASE WHEN @i <= 12 THEN 'Business' ELSE 'Economy' END, 'Available', CASE WHEN @i <= 12 THEN 2000000 ELSE 1500000 END);
    SET @i = @i + 1;
END