using Aspire.Qdrant.Client;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using RetailApp.ApiService;
using RetailApp.ApiService.Options;
using RetailApp.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.AddQdrantClient("retail-app-days");
builder.Services.Configure<AIOption>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddScoped<Kernel>(sp => 
    SetupKernelMemory.Build(
        sp.GetService<IOptions<AIOption>>(),
        sp.GetService<QdrantClientSettings>()));
builder.Services.AddScoped<IChatCompletionService>(sp => sp.GetService<Kernel>().Services.GetRequiredService<IChatCompletionService>());
builder.Services.AddScoped<IKernelMemory>(sp => sp.GetService<Kernel>().Services.GetRequiredService<IKernelMemory>());
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<ImportService>();
var app = builder.Build();

app.UseExceptionHandler();
app.MapGet("/search", async (string question, SearchService searchService) =>
{
    var answer = await searchService.Search(question);

    return Results.Ok(new Clothes { Result = answer.Result });
});

app.MapPost("/import/load", async (ImportService service) =>
{
    await service.Import();
});

app.MapDefaultEndpoints();
app.Run();
