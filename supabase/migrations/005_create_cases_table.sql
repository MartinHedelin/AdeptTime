-- Create cases table for storing Sager data
CREATE TABLE public.cases (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  case_number TEXT UNIQUE NOT NULL,
  title TEXT NOT NULL,
  description TEXT,
  team_id UUID REFERENCES public.teams(id) ON DELETE SET NULL,
  created_by UUID REFERENCES public.users(id) ON DELETE SET NULL,
  assigned_to UUID REFERENCES public.users(id) ON DELETE SET NULL,
  customer_id INTEGER, -- For now, keep as integer to match existing Customer model
  status TEXT NOT NULL DEFAULT 'Ny' CHECK (status IN ('Ny', 'I gang', 'Afventer', 'Færdig', 'Annulleret')),
  priority TEXT NOT NULL DEFAULT 'Medium' CHECK (priority IN ('Lav', 'Medium', 'Høj', 'Kritisk')),
  start_date DATE,
  end_date DATE,
  estimated_hours INTEGER,
  completed_hours INTEGER DEFAULT 0,
  
  -- Geofence data
  geofence_address TEXT,
  geofence_latitude DECIMAL(10, 8),
  geofence_longitude DECIMAL(11, 8),
  geofence_radius INTEGER DEFAULT 100, -- in meters
  
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create indexes for performance
CREATE INDEX idx_cases_team_id ON public.cases(team_id);
CREATE INDEX idx_cases_created_by ON public.cases(created_by);
CREATE INDEX idx_cases_assigned_to ON public.cases(assigned_to);
CREATE INDEX idx_cases_status ON public.cases(status);
CREATE INDEX idx_cases_start_date ON public.cases(start_date);

-- Create trigger for updated_at
CREATE TRIGGER update_cases_updated_at
    BEFORE UPDATE ON public.cases
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();
