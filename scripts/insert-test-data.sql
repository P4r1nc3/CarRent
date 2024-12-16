USE `car-rent`;

-- Insert into User table
INSERT INTO User (Name, Surname, Email, Password, Role) VALUES
('Jaros³aw', 'Janowicz', 'admin@example.com', 'password', 1),		-- Admin
('Adam', 'Nowak', 'customer@example.com', 'password', 2),           -- Customer 1
('Jan', 'Kowalski', 'customer1@example.com', 'password', 2),        -- Customer 2
('Micha³', 'Wiœniewski', 'employee@example.com', 'password', 3),    -- Employee
('Kacper', 'D¹browski', 'mechanic@example.com', 'password', 4);     -- Mechanic
