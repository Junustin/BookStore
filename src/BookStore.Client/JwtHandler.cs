using System;
using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace BookStore.Client;

public class JwtHandler : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;

    public JwtHandler(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Retrieve the stored JWT token string from localStorage
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        // If a token exists, attach it to the request HTTP header as "Bearer <token>"
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Send the modified request downstream to the WebAPI backend
        return await base.SendAsync(request, cancellationToken);
    }
}
