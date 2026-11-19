using System.Net.Http.Json;

namespace JaTak.App.Services;

/// <summary>
/// Handles communication with the JaTak WebAPI for user-related operations.
/// </summary>
public class UserApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the UserApiService class.
    /// </summary>
    public UserApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("JaTakApi");
    }

    /// <summary>
    /// Retrieves the currently authenticated user's information.
    /// </summary>
    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        return await _httpClient.GetFromJsonAsync<UserInfo>(
            "api/Users/me");
    }
}

/// <summary>
/// Represents the authenticated user's information returned by the API.
/// </summary>
public class UserInfo
{
    public int UserId { get; set; }

    public string? KeycloakId { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}