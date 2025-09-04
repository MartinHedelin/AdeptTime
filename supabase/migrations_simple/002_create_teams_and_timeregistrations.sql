-- Create teams table
CREATE TABLE public.teams (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  name TEXT UNIQUE NOT NULL,
  description TEXT,
  color TEXT DEFAULT '#3498db',
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create time_registrations table
CREATE TABLE public.time_registrations (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id UUID REFERENCES public.users(id) ON DELETE CASCADE,
  team_id UUID REFERENCES public.teams(id) ON DELETE SET NULL,
  date DATE NOT NULL,
  check_in TIME,
  check_out TIME,
  total_hours DECIMAL(4,2),
  time_bank DECIMAL(4,2) DEFAULT 0,
  status TEXT NOT NULL DEFAULT 'Afventer' CHECK (status IN ('Afventer', 'Godkendt', 'Afvist')),
  description TEXT,
  approved_by UUID REFERENCES public.users(id) ON DELETE SET NULL,
  approved_at TIMESTAMP WITH TIME ZONE,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create indexes for performance
CREATE INDEX idx_time_registrations_user_id ON public.time_registrations(user_id);
CREATE INDEX idx_time_registrations_team_id ON public.time_registrations(team_id);
CREATE INDEX idx_time_registrations_date ON public.time_registrations(date);
CREATE INDEX idx_time_registrations_status ON public.time_registrations(status);

-- Create trigger for updated_at on teams
CREATE TRIGGER update_teams_updated_at
    BEFORE UPDATE ON public.teams
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- Create trigger for updated_at on time_registrations
CREATE TRIGGER update_time_registrations_updated_at
    BEFORE UPDATE ON public.time_registrations
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();
