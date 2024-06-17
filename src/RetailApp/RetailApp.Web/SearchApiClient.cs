﻿namespace RetailApp.Web;

public class SearchApiClient(HttpClient httpClient)
{
    public async Task Import(CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync<string>("/import/load", "");
    }

    public async Task<List<Clothes>> GetClothes(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<Clothes>>("/search", cancellationToken);
    }
}