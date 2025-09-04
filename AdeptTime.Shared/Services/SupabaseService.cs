using Microsoft.Extensions.Logging;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;
using Supabase;

namespace AdeptTime.Shared.Services;

public class SupabaseService : ISupabaseService
{
    private readonly ILogger<SupabaseService> _logger;
    private readonly SupabaseSettings _settings;
    private Client? _client;

    public SupabaseService(ILogger<SupabaseService> logger, SupabaseSettings settings)
    {
        _logger = logger;
        _settings = settings;
    }

    public Client Client
    {
        get
        {
            if (_client == null)
                throw new InvalidOperationException("Supabase client is not initialized. Call InitializeAsync() first.");
            return _client;
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_client == null)
            {
                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = false,  // Disabled for Blazor WASM compatibility
                    AutoRefreshToken = true
                };

                _client = new Client(_settings.Url, _settings.Key, options);
                await _client.InitializeAsync();

                _logger.LogInformation("Supabase client initialized successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Supabase client");
            throw;
        }
    }

    public async Task<bool> IsConnectedAsync()
    {
        try
        {
            if (_client == null) return false;
            
            // Simple health check - try to get session info
            var session = _client.Auth.CurrentSession;
            return true; // If we get here without exception, we're connected
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Supabase connection check failed");
            return false;
        }
    }
}
