-- Thêm dữ liệu mẫu cho bảng Plane
INSERT INTO Plane (PlaneCode) VALUES ('A320');
INSERT INTO Plane (PlaneCode) VALUES ('B737');
INSERT INTO Plane (PlaneCode) VALUES ('A380');

-- Thêm dữ liệu mẫu cho bảng Seat
INSERT INTO Seat (SeatNumber, Class, Status, PlaneID) VALUES (1, 'Economy', 'Available', 1);
INSERT INTO Seat (SeatNumber, Class, Status, PlaneID) VALUES (2, 'Business', 'Available', 1);
INSERT INTO Seat (SeatNumber, Class, Status, PlaneID) VALUES (3, 'Economy', 'Reserved', 2);
INSERT INTO Seat (SeatNumber, Class, Status, PlaneID) VALUES (4, 'Economy', 'Available', 3);
INSERT INTO Seat (SeatNumber, Class, Status, PlaneID) VALUES (5, 'First', 'Available', 3);

-- Thêm dữ liệu mẫu cho bảng Pilot
INSERT INTO Pilot (Name) VALUES ('John Doe');
INSERT INTO Pilot (Name) VALUES ('Jane Smith');
INSERT INTO Pilot (Name) VALUES ('James Brown');

-- Thêm dữ liệu mẫu cho bảng FlightRoute
INSERT INTO FlightRoute (Location) VALUES ('New York');
INSERT INTO FlightRoute (Location) VALUES ('Los Angeles');
INSERT INTO FlightRoute (Location) VALUES ('Chicago');
INSERT INTO FlightRoute (Location) VALUES ('Miami');

-- Thêm dữ liệu mẫu cho bảng Flight
INSERT INTO Flight (PilotID, FlightNumber, DepartureLocation, DepartureTime, ArrivalLocation, ArrivalTime, FlightStatus, EmptySeat)
VALUES (1, 1001, 1, '2024-09-30 08:00:00', 2, '2024-09-30 11:00:00', 'Scheduled', 20);
INSERT INTO Flight (PilotID, FlightNumber, DepartureLocation, DepartureTime, ArrivalLocation, ArrivalTime, FlightStatus, EmptySeat)
VALUES (2, 1002, 3, '2024-10-01 09:30:00', 4, '2024-10-01 12:30:00', 'Scheduled', 15);
INSERT INTO Flight (PilotID, FlightNumber, DepartureLocation, DepartureTime, ArrivalLocation, ArrivalTime, FlightStatus, EmptySeat)
VALUES (3, 1003, 2, '2024-10-02 14:00:00', 1, '2024-10-02 18:00:00', 'Scheduled', 25);

-- Thêm dữ liệu mẫu cho bảng Pilot_Flight
INSERT INTO Pilot_Flight (PilotID, FlightID) VALUES (1, 1);
INSERT INTO Pilot_Flight (PilotID, FlightID) VALUES (2, 2);
INSERT INTO Pilot_Flight (PilotID, FlightID) VALUES (3, 3);

-- Thêm dữ liệu mẫu cho bảng Seat_Flight
INSERT INTO Seat_Flight (SeatID, FlightID, Status) VALUES (1, 1, 'Reserved');
INSERT INTO Seat_Flight (SeatID, FlightID, Status) VALUES (2, 2, 'Available');
INSERT INTO Seat_Flight (SeatID, FlightID, Status) VALUES (3, 3, 'Available');
INSERT INTO Seat_Flight (SeatID, FlightID, Status) VALUES (4, 1, 'Available');

-- Thêm dữ liệu mẫu cho bảng Membership
INSERT INTO Membership (Rank, Discount) VALUES ('Gold', 15.0);
INSERT INTO Membership (Rank, Discount) VALUES ('Silver', 10.0);
INSERT INTO Membership (Rank, Discount) VALUES ('Bronze', 5.0);

-- Thêm dữ liệu mẫu cho bảng User
INSERT INTO [User] (Email, Password, Gender, Nationality, Address, Fullname, DOB, Role, MembershipID, Status)
VALUES ('john.doe@example.com', 'password123', 'Male', 'USA', '123 Main St, New York, NY', 'John Doe', '1985-07-15', 'Customer', 1, 'Active');
INSERT INTO [User] (Email, Password, Gender, Nationality, Address, Fullname, DOB, Role, MembershipID, Status)
VALUES ('jane.smith@example.com', 'password456', 'Female', 'USA', '456 Elm St, Los Angeles, CA', 'Jane Smith', '1990-03-22', 'Customer', 2, 'Active');
INSERT INTO [User] (Email, Password, Gender, Nationality, Address, Fullname, DOB, Role, MembershipID, Status)
VALUES ('james.brown@example.com', 'password789', 'Male', 'USA', '789 Oak St, Chicago, IL', 'James Brown', '1975-11-08', 'Customer', 3, 'Active');

-- Thêm dữ liệu mẫu cho bảng Order
INSERT INTO [Order] (UserID, OrderDate, TripType, Status, TotalPrice)
VALUES (1, '2024-09-20', 'One-way', 'Confirmed', 200.0);
INSERT INTO [Order] (UserID, OrderDate, TripType, Status, TotalPrice)
VALUES (2, '2024-09-21', 'Round-trip', 'Confirmed', 350.0);

-- Thêm dữ liệu mẫu cho bảng OrderDetail
INSERT INTO OrderDetail (OrderID, FlightID, SeatID, SeatNumber, Status, TotalAmount)
VALUES (1, 1, 1, 1, 'Confirmed', 200.0);
INSERT INTO OrderDetail (OrderID, FlightID, SeatID, SeatNumber, Status, TotalAmount)
VALUES (2, 2, 2, 2, 'Confirmed', 350.0);

-- Thêm dữ liệu mẫu cho bảng Payment
INSERT INTO Payment (OrderDetailID, PaymentDate, PaymentMethod, Status, Amount)
VALUES (1, '2024-09-21', 'Credit Card', 'Paid', 200.0);
INSERT INTO Payment (OrderDetailID, PaymentDate, PaymentMethod, Status, Amount)
VALUES (2, '2024-09-22', 'Credit Card', 'Paid', 350.0);

-- Thêm dữ liệu mẫu cho bảng RefreshToken
INSERT INTO RefreshToken (UserID, ExpireDate, CreateDate, UpdateDate)
VALUES (1, '2024-12-31', '2024-09-20', '2024-09-20');
INSERT INTO RefreshToken (UserID, ExpireDate, CreateDate, UpdateDate)
VALUES (2, '2024-12-31', '2024-09-21', '2024-09-21');
