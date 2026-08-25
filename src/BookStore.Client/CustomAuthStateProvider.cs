using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BookStore.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    // Cached unauthenticated state (empty identity with no claims)
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthStateProvider(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

    // Called automatically by Blazor on startup or when auth state changes
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Read token from localStorage
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            
            // If missing, return an unauthenticated state immediately
            if (string.IsNullOrWhiteSpace(token))
                return new AuthenticationState(_anonymous);

            // Parse token string into claim objects and mark identity as authenticated with "jwt" scheme
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            // Fall back safely to unauthenticated on error (e.g., corrupted token)
            return new AuthenticationState(_anonymous);
        }

        
    }   

    // Call this upon successful login from Login.razor
    public async Task NotifyUserAuthenticationAsync(string token)
    {
        // Persist token to browser storage
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        
        // Re-build user identity from new token claims
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        
        // Notify all Blazor UI components (e.g., AuthorizeView, App.razor) to re-render
        NotifyAuthenticationStateChanged(authState);
    }

    // Call this upon logout
    public async Task NotifyUserLogoutAsync()
    {
        // Purge token from browser storage
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        
        // Notify Blazor components that the user is now anonymous
        var authState = Task.FromResult(new AuthenticationState(_anonymous));
        NotifyAuthenticationStateChanged(authState);
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        
        // JWT structure is Header.Payload.Signature; index 1 is Payload
        var payload = jwt.Split('.')[1];
        
        // Convert Base64Url payload to raw bytes
        var jsonBytes = ParseBase64WithoutPadding(payload);
        
        // Deserialize payload JSON key-value pairs
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        if (keyValuePairs is null) return claims;

        foreach (var kvp in keyValuePairs)
        {
            // Handle array values (e.g., user assigned multiple roles)
            if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    claims.Add(new Claim(kvp.Key, item.ToString()));
                }
            }
            else
            {
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString() ?? ""));
            }
        }
        return claims;
    }

    // Utility to fix Base64 padding (= / ==) and URL-safe characters (- / _)
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/'));
    }
}