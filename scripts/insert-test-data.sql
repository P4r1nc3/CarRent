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
('Audi', 'Q7', 2023, 400, 4),  -- Unavailable

-- BMW
('BMW', '3 Series', 2019, 180, 0),  -- Available
('BMW', '5 Series', 2020, 250, 0),  -- Available
('BMW', 'X1', 2021, 220, 0),        -- Available
('BMW', 'X3', 2022, 300, 0),        -- Available
('BMW', 'X5', 2022, 350, 3),        -- InService

-- Ford
('Ford', 'Fiesta', 2018, 120, 0),    -- Available
('Ford', 'Focus', 2019, 150, 0),     -- Available
('Ford', 'Escape', 2020, 200, 0),    -- Available
('Ford', 'Explorer', 2021, 300, 0),  -- Available
('Ford', 'Mustang', 2021, 450, 2),   -- Rented

-- Honda
('Honda', 'Civic', 2019, 180, 1),    -- Reserved
('Honda', 'Accord', 2020, 200, 0),   -- Available
('Honda', 'CR-V', 2021, 250, 0),     -- Available
('Honda', 'Pilot', 2022, 280, 0),    -- Available
('Honda', 'HR-V', 2023, 150, 0),     -- Available

-- Toyota
('Toyota', 'Corolla', 2019, 140, 0),      -- Available
('Toyota', 'Camry', 2020, 200, 0),        -- Available
('Toyota', 'RAV4', 2021, 220, 0),         -- Available
('Toyota', 'Highlander', 2022, 300, 0),   -- Available
('Toyota', 'Supra', 2023, 350, 0);        -- Available
