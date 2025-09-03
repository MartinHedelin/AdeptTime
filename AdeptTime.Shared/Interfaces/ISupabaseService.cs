using Supabase;

namespace parlayrunner.Shared.Interfaces;

public interface ISupabaseService
{
    Client Client { get; }
    Task InitializeAsync();
    Task<bool> IsConnectedAsync();
}
