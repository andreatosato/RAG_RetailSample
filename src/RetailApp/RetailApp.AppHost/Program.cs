var builder = DistributedApplication.CreateBuilder(args);

var apiKey = builder.AddParameter("apikey", secret: true);
var qdrant = builder.AddQdrant("retail-app-days", apiKey).WithBindMount("/qdrant/storage", "/qdrant/storage");

var apiService = builder.AddProject<Projects.RetailApp_ApiService>("apiservice")
    .WithReference(qdrant);

builder.AddProject<Projects.RetailApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
