USE `car-rent`;

-- Insert into Users table
INSERT INTO Users (Name, Surname, Email, Password, Role) VALUES
('Jaros³aw', 'Janowicz', 'admin@example.com', 'password', 0),      -- Admin
('Adam', 'Nowak', 'customer@example.com', 'password', 1),          -- Customer 1
('Jan', 'Kowalski', 'customer1@example.com', 'password', 1),       -- Customer 2
('Micha³', 'Wiœniewski', 'employee@example.com', 'password', 2),   -- Employee
('Kacper', 'D¹browski', 'mechanic@example.com', 'password', 3);    -- Mechanic

-- Insert into Cars table
INSERT INTO Cars (Make, Model, Year, HorsePower, CarState) VALUES
-- Audi
('Audi', 'A4', 2020, 190, 0),  -- Available
('Audi', 'A6', 2021, 250, 0),  -- Available
('Audi', 'Q3', 2022, 200, 0),  -- Available
('Audi', 'Q5', 2023, 300, 0),  -- Available
('Audi', 'Q7', 2023, 400, 1),  -- Reserved

-- BMW
('BMW', '3 Series', 2019, 180, 0),  -- Available
('BMW', '5 Series', 2020, 250, 0),  -- Available
('BMW', 'X1', 2021, 220, 0),        -- Available
('BMW', 'X3', 2022, 300, 0),        -- Available
('BMW', 'X5', 2022, 350, 2),        -- Rented

-- Ford
('Ford', 'Fiesta', 2018, 120, 0),    -- Available
('Ford', 'Focus', 2019, 150, 0),     -- Available
('Ford', 'Escape', 2020, 200, 0),    -- Available
('Ford', 'Explorer', 2021, 300, 0),  -- Available
('Ford', 'Mustang', 2021, 450, 1),   -- Reserved

-- Honda
('Honda', 'Civic', 2019, 180, 1),    -- Reserved
('Honda', 'Accord', 2020, 200, 0),   -- Available
('Honda', 'CR-V', 2021, 250, 0),     -- Available
('Honda', 'Pilot', 2022, 280, 2),    -- Rented
('Honda', 'HR-V', 2023, 150, 0),     -- Available

-- Toyota
('Toyota', 'Corolla', 2019, 140, 0),      -- Available
('Toyota', 'Camry', 2020, 200, 2),        -- Rented
('Toyota', 'RAV4', 2021, 220, 0),         -- Available
('Toyota', 'Highlander', 2022, 300, 0),   -- Available
('Toyota', 'Supra', 2023, 350, 2);        -- Rented

-- Insert into Requests table
INSERT INTO Requests (CarId, UserId, StartDate, EndDate, IsAccepted) VALUES
-- IsAccepted = FALSE
(5, 2, '2025-03-01', '2025-03-10', FALSE),  -- Audi Q7 Reserved
(10, 3, '2025-03-02', '2025-03-12', FALSE), -- Honda Civic Reserved
(15, 2, '2025-03-05', '2025-03-15', FALSE), -- BMW X5 Reserved
(12, 3, '2025-03-07', '2025-03-17', FALSE), -- Ford Mustang Reserved

-- IsAccepted = TRUE
(20, 2, '2025-03-03', '2025-03-13', TRUE),  -- Toyota Supra Rented
(15, 3, '2025-03-04', '2025-03-14', TRUE),  -- BMW X5 Rented
(19, 2, '2025-03-06', '2025-03-16', TRUE),  -- Honda Pilot Rented
(18, 3, '2025-03-08', '2025-03-18', TRUE);  -- Toyota Camry Rented
