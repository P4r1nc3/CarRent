﻿USE `car-rent`;

-- ====================================
-- Insert into Users table
-- ====================================
INSERT INTO Users (Name, Surname, Email, Password, Role) VALUES
('Jarosław', 'Janowicz', 'admin@example.com', 'password', 0),      -- Admin
('Adam', 'Nowak', 'customer@example.com', 'password', 1),           -- Customer 1
('Jan', 'Kowalski', 'customer1@example.com', 'password', 1),        -- Customer 2
('Michał', 'Wiśniewski', 'employee@example.com', 'password', 2),    -- Employee
('Kacper', 'Dąbrowski', 'mechanic@example.com', 'password', 3);     -- Mechanic

-- ====================================
-- Insert into Cars table
-- ====================================
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
('Toyota', 'Supra', 2023, 350, 2),        -- Rented

-- inService (These will have IDs 26 through 30)
('Audi', 'A8', 2024, 500, 3),          -- In Service (ID = 26)
('BMW', 'X7', 2024, 400, 3),           -- In Service (ID = 27)
('Ford', 'Ranger', 2024, 300, 3),      -- In Service (ID = 28)
('Honda', 'Odyssey', 2024, 250, 3),    -- In Service (ID = 29)
('Toyota', 'Sienna', 2024, 220, 3);    -- In Service (ID = 30)

-- ====================================
-- Insert into Requests table
-- ====================================
INSERT INTO Requests (CarId, UserId, StartDate, EndDate, RequestState) VALUES
-- IsAccepted = FALSE
(5, 2, '2025-03-01', '2025-03-10', 1),  -- Audi Q7 Reserved
(10, 3, '2025-03-02', '2025-03-12', 1), -- Honda Civic Reserved
(15, 2, '2025-03-05', '2025-03-15', 1), -- BMW X5 Reserved
(12, 3, '2025-03-07', '2025-03-17', 1), -- Ford Mustang Reserved

-- IsAccepted = TRUE
(20, 2, '2025-03-03', '2025-03-13', 2),  -- Toyota Supra Rented
(15, 3, '2025-03-04', '2025-03-14', 2),  -- BMW X5 Rented
(19, 2, '2025-03-06', '2025-03-16', 2),  -- Honda Pilot Rented
(18, 3, '2025-03-08', '2025-03-18', 2);  -- Toyota Camry Rented

-- ====================================
-- Insert into Repairs table
-- (Example: creating repair records for some inService cars)
-- ====================================
INSERT INTO Repairs (CarId, RepairSummary, TotalCost, RepairDate) VALUES
-- For Audi A8 (CarId = 26)
(26, 'Replaced engine oil, oil filter and repaired brakes. Parts and labor included.', 350.00, '2025-04-15'),
-- For BMW X7 (CarId = 27)
(27, 'Repaired suspension and replaced worn-out tires. Complete service performed.', 1200.00, '2025-04-18');

-- ====================================
-- Insert into RepairItems table
-- (Each repair record can have multiple repair items)
-- ====================================
-- For Repair record with Id = 1 (Audi A8 repair)
INSERT INTO RepairItems (RepairId, Description, Cost, Quantity) VALUES
(1, 'Engine oil replacement', 100.00, 1),
(1, 'Oil filter', 50.00, 1),
(1, 'Brake repair', 100.00, 1),
(1, 'Labor', 50.00, 1);

-- For Repair record with Id = 2 (BMW X7 repair)
INSERT INTO RepairItems (RepairId, Description, Cost, Quantity) VALUES
(2, 'Suspension repair', 400.00, 1),
(2, 'Tire replacement', 300.00, 1),
(2, 'Wheel alignment', 200.00, 1),
(2, 'Labor', 300.00, 1);