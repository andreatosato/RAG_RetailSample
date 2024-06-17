
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
    public static Kernel Build(IOptions<AIOption> options, QdrantClientSettings qdrantClientSettings)
    {
        var option = options.Value!;
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
            .WithQdrantMemoryDb(qdrantClientSettings.Endpoint.ToString()) // "http://localhost:6333/"
            .Build<Microsoft.KernelMemory.MemoryServerless>();

        var builder = Kernel.CreateBuilder();

        builder.Services.AddLogging(builder => builder.AddConsole());
        builder.Services
            .AddAzureOpenAIChatCompletion(option.Chat.Deployment, option.Endpoint, option.ApiKey);

        var kernel = builder.Build();
        var km = kernel.GetRequiredService<IKernelMemory>();
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        //var qdrant = kernel.Services.GetRequiredService<IMemoryDb>();
        return kernel;
    }
}
