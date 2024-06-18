
using Aspire.Qdrant.Client;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.MemoryStorage;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Qdrant.Client;
using RetailApp.ApiService.Options;

namespace RetailApp.ApiService;

public static class SetupKernelMemory
{
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, AIOption option, string qdrantConnectionString)
    {
        var qdrantArray = qdrantConnectionString.Split(";").Select(t => t.Replace("Endpoint=", "").Replace("Key=", "")).ToArray();
        var qdrantEndpoint = qdrantArray[0];
        var qdrantApiKey = qdrantArray[1];
        var kernelMemory = new KernelMemoryBuilder(services)
            .WithAzureOpenAITextEmbeddingGeneration(new()
            {
                APIKey = option.ApiKey,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                Deployment = option.Embedding.Deployment,
                Endpoint = option.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                MaxTokenTotal = int.Parse(option.Embedding.MaxTokens)
            })
            .WithAzureOpenAITextGeneration(new()
            {
                APIKey = option.ApiKey,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                Deployment = option.Chat.Deployment,
                Endpoint = option.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
                MaxTokenTotal = int.Parse(option.Chat.MaxTokens)
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
            .WithQdrantMemoryDb(qdrantEndpoint, qdrantApiKey) // "http://localhost:6333/"
            .Build<MemoryServerless>();

        var builder = Kernel.CreateBuilder();
        var semanticKernelBuild = builder.Build();
        //services.AddScoped<Kernel>(sp => semanticKernelBuild);
        return services;
    }
    public static Kernel Build(IOptions<AIOption> options, string qdrantConnectionString)
    {
        var option = options.Value!;
        var qdrantArray = qdrantConnectionString.Split(";").Select(t => t.Replace("Endpoint=", "").Replace("Key=", "")).ToArray();
        var qdrantEndpoint = qdrantArray[0];
        var qdrantApiKey = qdrantArray[1];
        var kernelMemory = new KernelMemoryBuilder()
            .WithAzureOpenAITextEmbeddingGeneration(new()
            {
                APIKey = option.ApiKey,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                Deployment = option.Embedding.Deployment,
                Endpoint = option.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                MaxTokenTotal = int.Parse(option.Embedding.MaxTokens)
            })
            .WithAzureOpenAITextGeneration(new()
            {
                APIKey = option.ApiKey,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                Deployment = option.Chat.Deployment,
                Endpoint = option.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
                MaxTokenTotal = int.Parse(option.Chat.MaxTokens)
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
            .WithQdrantMemoryDb(qdrantEndpoint, qdrantApiKey) // "http://localhost:6333/"
            .Build<Microsoft.KernelMemory.MemoryServerless>();

        var builder = Kernel.CreateBuilder();

        builder.Services.AddLogging(builder => builder.AddConsole());
        builder.Services
            .AddAzureOpenAIChatCompletion(option.Chat.Deployment, option.Endpoint, option.ApiKey);
        builder.Services.AddScoped<IKernelMemory>(sp => kernelMemory);
        
        var kernel = builder.Build();
        var km = kernel.GetRequiredService<IKernelMemory>();
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        return kernel;
    }
}
