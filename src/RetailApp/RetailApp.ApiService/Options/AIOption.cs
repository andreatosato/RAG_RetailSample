namespace RetailApp.ApiService.Options;

public class AIOption
{
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public ModelOption Chat { get; set; } = null!;
    public ModelOption Embedding { get; set; } = null!;
}

public class ModelOption
{
    public string Deployment { get; set; } = string.Empty;
    public string MaxTokens { get; set; } = string.Empty;
}
