using Microsoft.AspNetCore.Mvc;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using RetailApp.ApiService.Options;
using RetailApp.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
var qdrantConnectionstring = builder.Configuration.GetConnectionString("retail-app-days");
var qdrantArray = qdrantConnectionstring.Split(";").Select(t => t.Replace("Endpoint=", "").Replace("Key=", "")).ToArray();
var uri = new UriBuilder(qdrantArray[0]);
uri.Port++;
var qdrantEndpoint = uri.ToString();
var qdrantApiKey = qdrantArray[1];
var aiOption = builder.Configuration.GetSection("OpenAI").Get<AIOption>()!;

builder.AddQdrantClient("retail-app-days");
builder.Services.AddKernelMemory(c => c
            .WithAzureOpenAITextEmbeddingGeneration(new()
            {
                APIKey = aiOption.ApiKey,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                Deployment = aiOption.Embedding.Deployment,
                Endpoint = aiOption.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                MaxTokenTotal = int.Parse(aiOption.Embedding.MaxTokens)
            })
            .WithAzureOpenAITextGeneration(new()
            {
                APIKey = aiOption.ApiKey,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                Deployment = aiOption.Chat.Deployment,
                Endpoint = aiOption.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
                MaxTokenTotal = int.Parse(aiOption.Chat.MaxTokens)
            })
            .WithSearchClientConfig(new()
            {
                EmptyAnswer = "Mi dispiace, Non ho trovato nessuna informazione che possa essere utilizzata per risponderti. Prova a riformulare la richiesta.",
                MaxMatchesCount = 25,
                AnswerTokens = 800
            })
            .WithCustomTextPartitioningOptions(new()
            {
                // Defines the properties that are used to split the documents in chunks.
                MaxTokensPerParagraph = 1500,
                MaxTokensPerLine = 300,
                OverlappingTokens = 100
            })
            .WithQdrantMemoryDb(qdrantEndpoint, qdrantApiKey)
            .Build<MemoryServerless>());
builder.Services.AddAzureOpenAIChatCompletion(aiOption.Chat.Deployment, aiOption.Endpoint, aiOption.ApiKey);
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<ImportService>();
var app = builder.Build();

app.UseExceptionHandler();
app.MapPost("/search", async ([FromBody]QuestionRequest question, SearchService searchService) =>
{
    MemoryAnswer answer = await searchService.Search(question.QuestionUser);
    if(!answer?.NoResult ?? false)
        return Results.Ok(new Clothes
        {
            Citations = answer?.RelevantSources.Select(x => new Citation
            {
                DocumentId = x.DocumentId,
                FileId = x.FileId,
                Index = x.Index,
                Link = x.Link,
                SourceContentType = x.SourceContentType,
                SourceName = x.SourceName,
                SourceUrl = x.SourceUrl
            })
            .ToList()
        });
    
    
    return Results.NotFound();
});

app.MapPost("/import/load", async (ImportService service) =>
{
    await service.Import();
});

app.MapDefaultEndpoints();
app.Run();

public class QuestionRequest
{
    public string QuestionUser { get; set; } = null!;
}
