-- Simple seed data for development/testing
-- Easy to understand test users with proper password hashes and avatar URLs

INSERT INTO public.users (email, password_hash, user_type_id, name, phone_number, address, avatar_url, user_state) VALUES
('admin@test.com', 'ZjVjOGQ4OWE4YzhjZDFlYjMwMmQxNzE4YTc3YTZhOTc1NzA2NjY0NDQyNDk1NzMyNzY0MzE5YmNkODNlMGM4Nw==', 1, 'Admin User', '+1234567890', '123 Admin Street', 'https://plus.unsplash.com/premium_photo-1671656349322-41de944d259b?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHhzZWFyY2h8MXx8cG9ydHJhaXR8ZW58MHx8MHx8fDA%3D', 'active'),
('worker@test.com', 'ZjVjOGQ4OWE4YzhjZDFlYjMwMmQxNzE4YTc3YTZhOTc1NzA2NjY0NDQyNDk1NzMyNzY0MzE5YmNkODNlMGM4Nw==', 0, 'Worker User', '+1234567891', '456 Worker Ave', 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHhwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 'active'),
('john@test.com', 'ZjVjOGQ4OWE4YzhjZDFlYjMwMmQxNzE4YTc3YTZhOTc1NzA2NjY0NDQyNDk1NzMyNzY0MzE5YmNkODNlMGM4Nw==', 0, 'John Doe', '+1234567892', '789 John Street', 'https://images.unsplash.com/photo-1534528741775-53994a69daeb?q=80&w=764&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHhwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 'active');

-- Insert dummy teams data
INSERT INTO public.teams (name, description, color) VALUES
('All Teams', 'All team members', '#6c757d'),
('Team London', 'London office team', '#007bff'),
('Team Dublin', 'Dublin office team', '#28a745'),
('Team Copenhagen', 'Copenhagen office team', '#dc3545'),
('Team Berlin', 'Berlin office team', '#ffc107'),
('Team Stockholm', 'Stockholm office team', '#17a2b8');

-- Insert time registrations with proper relationships
DO $$
DECLARE
    team_london_id UUID;
    team_dublin_id UUID;
    team_copenhagen_id UUID;
    team_berlin_id UUID;
    admin_user_id UUID;
    worker_user_id UUID;
    john_user_id UUID;
BEGIN
    -- Get team IDs
    SELECT id INTO team_london_id FROM public.teams WHERE name = 'Team London';
    SELECT id INTO team_dublin_id FROM public.teams WHERE name = 'Team Dublin';
    SELECT id INTO team_copenhagen_id FROM public.teams WHERE name = 'Team Copenhagen';
    SELECT id INTO team_berlin_id FROM public.teams WHERE name = 'Team Berlin';
    
    -- Get user IDs
    SELECT id INTO admin_user_id FROM public.users WHERE email = 'admin@test.com';
    SELECT id INTO worker_user_id FROM public.users WHERE email = 'worker@test.com';
    SELECT id INTO john_user_id FROM public.users WHERE email = 'john@test.com';
    
    -- Insert dummy time registrations
    -- Admin user (Team London) - Multiple entries
    INSERT INTO public.time_registrations (user_id, team_id, date, check_in, check_out, total_hours, time_bank, status, description) VALUES
    (admin_user_id, team_london_id, '2024-01-22', '08:00', '17:15', 9.25, 1.25, 'Afventer', 'Regular work day - project management'),
    (admin_user_id, team_london_id, '2024-01-23', '08:00', '17:00', 9.00, 1.00, 'Godkendt', 'Team meetings and code reviews'),
    (admin_user_id, team_london_id, '2024-01-24', '08:30', '17:30', 9.00, 1.00, 'Afvist', 'Late start due to client meeting'),
    (admin_user_id, team_london_id, '2024-01-25', '08:00', '16:45', 8.75, 0.75, 'Afventer', 'Development work');
    
    -- Worker user (Team Dublin) - Multiple entries  
    INSERT INTO public.time_registrations (user_id, team_id, date, check_in, check_out, total_hours, time_bank, status, description, approved_by) VALUES
    (worker_user_id, team_dublin_id, '2024-01-22', '08:00', '17:00', 9.00, 1.00, 'Godkendt', 'Client support and bug fixes', admin_user_id),
    (worker_user_id, team_dublin_id, '2024-01-23', '08:15', '17:15', 9.00, 1.00, 'Afventer', 'Feature development', null),
    (worker_user_id, team_dublin_id, '2024-01-24', '08:00', '16:30', 8.50, 0.50, 'Afvist', 'Short day - doctor appointment', null);
    
    -- John user (Team Copenhagen) - Multiple entries
    INSERT INTO public.time_registrations (user_id, team_id, date, check_in, check_out, total_hours, time_bank, status, description) VALUES
    (john_user_id, team_copenhagen_id, '2024-01-22', '09:00', '18:00', 9.00, 1.00, 'Afventer', 'Database optimization work'),
    (john_user_id, team_copenhagen_id, '2024-01-23', '08:30', '17:30', 9.00, 1.00, 'Godkendt', 'API development and testing'),
    (john_user_id, team_copenhagen_id, '2024-01-24', '08:00', '17:45', 9.75, 1.75, 'Afventer', 'Overtime for project deadline'),
    (john_user_id, team_copenhagen_id, '2024-01-25', '08:00', '17:00', 9.00, 1.00, 'Godkendt', 'Code review and documentation');
    
    -- Additional entries for Berlin team (using existing users)
    INSERT INTO public.time_registrations (user_id, team_id, date, check_in, check_out, total_hours, time_bank, status, description) VALUES
    (admin_user_id, team_berlin_id, '2024-01-26', '08:00', '17:30', 9.50, 1.50, 'Afventer', 'Cross-team collaboration'),
    (worker_user_id, team_berlin_id, '2024-01-26', '08:30', '17:00', 8.50, 0.50, 'Afventer', 'Remote work from Berlin office');
    
    -- Insert dummy cases data
    INSERT INTO public.cases (case_number, title, description, team_id, created_by, assigned_to, customer_id, status, priority, start_date, end_date, estimated_hours, completed_hours, geofence_address, geofence_latitude, geofence_longitude, geofence_radius) VALUES
    ('SAG-2024-0001', 'VVS Installation', 'Installation af nyt VVS system i køkken og bad', team_london_id, admin_user_id, worker_user_id, 1, 'I gang', 'Høj', '2024-01-22', '2024-01-25', 32, 16, 'Vesterbro 36, 2300 København S', 55.6761, 12.5683, 150),
    ('SAG-2024-0002', 'Elektrisk Vedligeholdelse', 'Rutine tjek af elektriske installationer', team_dublin_id, admin_user_id, john_user_id, 2, 'Ny', 'Medium', '2024-01-25', '2024-01-27', 16, 0, 'Nørrebro 12, 2200 København N', 55.6867, 12.5700, 100),
    ('SAG-2024-0003', 'Byggeprojekt Konsultation', 'Konsultation vedrørende nyt byggeprojekt', team_copenhagen_id, admin_user_id, null, 3, 'Afventer', 'Medium', '2024-01-28', '2024-02-01', 24, 0, 'Østerbro 45, 2100 København Ø', 55.7058, 12.5691, 200),
    ('SAG-2024-0004', 'Akut VVS Reparation', 'Akut reparation af vandrør i kælder', team_london_id, worker_user_id, worker_user_id, 1, 'Færdig', 'Kritisk', '2024-01-20', '2024-01-21', 8, 8, 'Amager Boulevard 70, 2300 København S', 55.6586, 12.5912, 75),
    ('SAG-2024-0005', 'Preventiv Vedligeholdelse', 'Årlig vedligeholdelse af ventilationssystem', team_berlin_id, admin_user_id, john_user_id, 2, 'I gang', 'Lav', '2024-01-26', '2024-01-30', 40, 12, 'Frederiksberg Allé 15, 1820 Frederiksberg', 55.6736, 12.5348, 125);
    
END $$;