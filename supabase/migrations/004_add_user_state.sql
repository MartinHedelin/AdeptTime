-- Add user_state column to track active/invited status
ALTER TABLE public.users ADD COLUMN user_state TEXT NOT NULL DEFAULT 'active' CHECK (user_state IN ('active', 'invited', 'inactive'));
