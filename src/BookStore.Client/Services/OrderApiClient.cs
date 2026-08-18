using System.Net.Http.Json;
using BookStore.Shared;

namespace BookStore.Client.Services;

public class OrderApiClient(HttpClient httpClient)
{
    public async Task<List<OrderDto>> GetOrdersAsync()=>
            await httpClient.GetFromJsonAsync<List<OrderDto>>("api/orders") ?? [];
         
}
