using System;
using System.Net.Http.Json;
using BookStore.Shared.Dtos;

namespace BookStore.Client.Services;

public class BookApiClient(HttpClient httpClient)
{
    public async Task<List<BookDto>> GetBooksAsync()=>
        await httpClient.GetFromJsonAsync<List<BookDto>>("api/books") ?? [];

    public async Task<bool> CreateBookAsync(CreateBookDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("api/books", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBookAsync(int id, UpdateBookDto dto)
    {
        var response = await httpClient.PutAsJsonAsync($"api/books/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/books/{id}");
        return response.IsSuccessStatusCode;
    }
}
