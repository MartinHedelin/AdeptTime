-- Simple users table for development/staging
-- No complex RLS policies, just basic functionality
-- Matches the C# User model structure

CREATE TABLE public.users (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  email TEXT UNIQUE NOT NULL,
  password_hash TEXT NOT NULL,
  user_type_id INTEGER NOT NULL DEFAULT 0 CHECK (user_type_id IN (0, 1)), -- 0 = worker, 1 = admin
  name TEXT NOT NULL,
  phone_number TEXT,
  address TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Simple index for performance
CREATE INDEX idx_users_email ON public.users(email);

-- No RLS for development - keep it simple
-- (You can add proper security policies later when moving to production)

-- Simple function to update updated_at timestamp
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Create trigger for updated_at
CREATE TRIGGER update_users_updated_at 
BEFORE UPDATE ON public.users 
FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
