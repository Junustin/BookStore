using System.Net.Http.Json;
using BookStore.Shared.Dtos;
using BookStore.Client.Models;

namespace BookStore.Client.Services;

public class OrderApiClient(HttpClient httpClient)
{
    public async Task<List<OrderDto>> GetOrdersAsync()=>
            await httpClient.GetFromJsonAsync<List<OrderDto>>("api/orders") ?? [];

    public async Task<(bool IsSuccess,String? ErrorMessage)> CreateOrderAsync(CreateOrderDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("api/orders", dto);
        if (response.IsSuccessStatusCode)
        {
            return(true,null);
        }

        var error = await response.Content.ReadAsStringAsync();
        return(false, string.IsNullOrWhiteSpace(error) ? "Order failed": error);
    } 
}
