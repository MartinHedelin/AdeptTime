using Supabase;

namespace AdeptTime.Shared.Interfaces;

public interface ISupabaseService
{
    Client Client { get; }
    Task InitializeAsync();
    Task<bool> IsConnectedAsync();
    Task<Client> GetClientAsync();
}
