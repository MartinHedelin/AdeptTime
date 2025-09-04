-- Insert dummy teams data
INSERT INTO public.teams (name, description, color) VALUES
('All Teams', 'All team members', '#6c757d'),
('Team London', 'London office team', '#007bff'),
('Team Dublin', 'Dublin office team', '#28a745'),
('Team Copenhagen', 'Copenhagen office team', '#dc3545'),
('Team Berlin', 'Berlin office team', '#ffc107'),
('Team Stockholm', 'Stockholm office team', '#17a2b8');

-- Get team IDs for reference
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
    (worker_user_id, team_dublin_id, '2024-01-23', '08:15', '17:15', 9.00, 1.00, 'Afventer', 'Feature development'),
    (worker_user_id, team_dublin_id, '2024-01-24', '08:00', '16:30', 8.50, 0.50, 'Afvist', 'Short day - doctor appointment');
    
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
    
END $$;
