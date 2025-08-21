-- Seed data for testing

-- Insert test users with hashed passwords (password is "password123" for all)
INSERT INTO public.users (id, email, password_hash, user_type_id, name, phone_number, address) VALUES
(
  'a1b2c3d4-e5f6-7890-abcd-ef1234567890'::uuid,
  'admin@company.com',
  'v8xk5y/FzKvV4+4JhZf5JnrQGrZLM/1k3Zq1YK8FzKvV', -- Hashed: password123
  1,
  'Admin User',
  '+1234567890',
  '123 Admin Street, Admin City'
),
(
  'b2c3d4e5-f6a7-8901-bcde-f12345678901'::uuid,
  'worker@company.com',
  'v8xk5y/FzKvV4+4JhZf5JnrQGrZLM/1k3Zq1YK8FzKvV', -- Hashed: password123
  0,
  'Worker User',
  '+1234567891',
  '456 Worker Avenue, Worker Town'
),
(
  'c3d4e5f6-a7b8-9012-cdef-123456789012'::uuid,
  'john.doe@company.com',
  'v8xk5y/FzKvV4+4JhZf5JnrQGrZLM/1k3Zq1YK8FzKvV', -- Hashed: password123
  0,
  'John Doe',
  '+1234567892',
  '789 Regular Street, Normal City'
);
