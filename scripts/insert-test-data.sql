USE `car-rent`;

-- Insert into User table
INSERT INTO Users (Name, Surname, Email, Password, Role) VALUES
('Jaros³aw', 'Janowicz', 'admin@example.com', 'password', 0),		-- Admin
('Adam', 'Nowak', 'customer@example.com', 'password', 1),           -- Customer 1
('Jan', 'Kowalski', 'customer1@example.com', 'password', 1),        -- Customer 2
('Micha³', 'Wiœniewski', 'employee@example.com', 'password', 2),    -- Employee
('Kacper', 'D¹browski', 'mechanic@example.com', 'password', 3);     -- Mechanic
