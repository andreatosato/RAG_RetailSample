using Microsoft.KernelMemory;
using System.Net.Http.Json;
using System.Web;

namespace RetailApp.Web;

public class SearchApiClient(HttpClient httpClient)
{
    public async Task Import(CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync<string>("/import/load", "");
    }

    public async Task<Clothes> GetClothes(string question, CancellationToken cancellationToken = default)
    {
        var content = await httpClient.PostAsJsonAsync("/search", new QuestionRequest { QuestionUser = question }, cancellationToken);
        return await content.Content.ReadFromJsonAsync<Clothes>();
    }
}

public class QuestionRequest
{
    public string QuestionUser { get; set; } = null!;
}