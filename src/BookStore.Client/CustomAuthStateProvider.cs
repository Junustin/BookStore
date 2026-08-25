using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BookStore.Client;

public class CustomAuthStateProvider(IJSRuntime jsRuntime) : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }  

    public async Task Login(string token)
    {
        token = token.Trim('"');
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public async Task Logout()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }
        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? "")) ?? [];
    }
} 

public class JwtHandler(IJSRuntime jsRuntime) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
        }

        return await base.SendAsync(request, cancellationToken);
    }
}