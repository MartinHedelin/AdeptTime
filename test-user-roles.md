# User Role Testing Guide

## 🧪 **Testing User Creation & Role Detection**

### **Test Users Available:**
```csharp
// Admin User (user_type_id = 1)
Email: admin@test.com
Password: password123
IsAdmin: true ✅
IsWorker: false

// Worker User (user_type_id = 0)  
Email: worker@test.com
Password: password123
IsAdmin: false
IsWorker: true ✅

// Regular User (user_type_id = 0)
Email: john@test.com  
Password: password123
IsAdmin: false
IsWorker: true ✅
```

### **How Role Detection Works:**

#### 1. **Signup Process:**
```csharp
// In Signup.razor
var newUser = new User
{
    Email = signupModel.Email,
    UserTypeId = signupModel.UserTypeId,  // 0 = worker, 1 = admin
    Name = signupModel.Name,
    // ... other fields
};

// Creates database record with user_type_id
```

#### 2. **Login Process:**
```csharp
// In Login.razor
var user = await UserService.GetUserByEmailAsync(email);

// Role detection using User model helper properties
UserRoleService.SetCurrentUser(email, user.IsAdmin);

// User.cs helper properties:
public bool IsAdmin => UserTypeId == 1;   // ✅ Admin check
public bool IsWorker => UserTypeId == 0;  // ✅ Worker check
```

#### 3. **Role Checking in Components:**
```csharp
@inject UserRoleService UserRoleService

// Check if user is admin
@if (UserRoleService.IsAdministrator)
{
    <p>Welcome Admin! You have full access.</p>
    <!-- Show admin-only features -->
}
else
{
    <p>Welcome Worker! You have standard access.</p>
    <!-- Show worker features -->
}
```

### **Database Schema (Simplified):**
```sql
CREATE TABLE public.users (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  email TEXT UNIQUE NOT NULL,
  password_hash TEXT NOT NULL,
  user_type_id INTEGER NOT NULL DEFAULT 0,  -- 0 = worker, 1 = admin
  name TEXT NOT NULL,
  phone_number TEXT,
  address TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

### **✅ What's Working:**
- ✅ Signup creates users with correct `user_type_id`
- ✅ Login retrieves user and detects role via `user.IsAdmin`
- ✅ Role state is managed in `UserRoleService`
- ✅ Components can check `UserRoleService.IsAdministrator`
- ✅ Database schema matches C# model expectations

### **🧭 Usage in Components:**
```csharp
// For showing admin-only content
@if (UserRoleService.IsAdministrator)
{
    <button class="btn btn-danger">Delete All Data</button>
    <a href="/admin/users">Manage Users</a>
}

// For worker-only content  
@if (!UserRoleService.IsAdministrator)
{
    <p>Submit your timesheet below:</p>
    <button class="btn btn-primary">Clock In</button>
}

// Always available
<button class="btn btn-secondary">View Profile</button>
```
