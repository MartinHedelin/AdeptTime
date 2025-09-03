-- Create custom users table (separate from auth.users)
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

-- Create user_types lookup table for clarity
CREATE TABLE public.user_types (
  id INTEGER PRIMARY KEY,
  name TEXT NOT NULL,
  description TEXT
);

-- Insert user types
INSERT INTO public.user_types (id, name, description) VALUES
(0, 'Worker', 'Regular worker with standard permissions'),
(1, 'Administrator', 'Admin with full system access');

-- Add foreign key constraint
ALTER TABLE public.users 
ADD CONSTRAINT fk_user_type 
FOREIGN KEY (user_type_id) REFERENCES public.user_types(id);

-- Create indexes for performance
CREATE INDEX idx_users_email ON public.users(email);
CREATE INDEX idx_users_user_type ON public.users(user_type_id);

-- Enable Row Level Security
ALTER TABLE public.users ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.user_types ENABLE ROW LEVEL SECURITY;

-- Create policies
-- Users can read their own data
CREATE POLICY "Users can read own data" ON public.users
FOR SELECT USING (auth.uid()::TEXT = id::TEXT);

-- Admins can read all users
CREATE POLICY "Admins can read all users" ON public.users
FOR SELECT USING (
  EXISTS (
    SELECT 1 FROM public.users 
    WHERE id::TEXT = auth.uid()::TEXT 
    AND user_type_id = 1
  )
);

-- Users can update their own data (except user_type_id)
CREATE POLICY "Users can update own data" ON public.users
FOR UPDATE USING (auth.uid()::TEXT = id::TEXT)
WITH CHECK (auth.uid()::TEXT = id::TEXT);

-- Admins can update any user
CREATE POLICY "Admins can update any user" ON public.users
FOR UPDATE USING (
  EXISTS (
    SELECT 1 FROM public.users 
    WHERE id::TEXT = auth.uid()::TEXT 
    AND user_type_id = 1
  )
);

-- Allow reading user_types for all authenticated users
CREATE POLICY "Authenticated users can read user types" ON public.user_types
FOR SELECT USING (auth.role() = 'authenticated');

-- Create function to update updated_at timestamp
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
