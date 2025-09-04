-- Simple seed data for development/testing
-- Easy to understand test users with proper password hashes

INSERT INTO public.users (email, password_hash, user_type_id, name, phone_number, address) VALUES
('admin@test.com', 'ZjVjOGQ4OWE4YzhjZDFlYjMwMmQxNzE4YTc3YTZhOTc1NzA2NjY0NDQyNDk1NzMyNzY0MzE5YmNkODNlMGM4Nw==', 1, 'Admin User', '+1234567890', '123 Admin Street'),
('worker@test.com', 'ZjVjOGQ4OWE4YzhjZDFlYjMwMmQxNzE4YTc3YTZhOTc1NzA2NjY0NDQyNDk1NzMyNzY0MzE5YmNkODNlMGM4Nw==', 0, 'Worker User', '+1234567891', '456 Worker Ave'),
('john@test.com', 'ZjVjOGQ4OWE4YzhjZDFlYjMwMmQxNzE4YTc3YTZhOTc1NzA2NjY0NDQyNDk1NzMyNzY0MzE5YmNkODNlMGM4Nw==', 0, 'John Doe', '+1234567892', '789 Normal St');

-- Note: All passwords are "password123" (same hash as used in the complex schema)
