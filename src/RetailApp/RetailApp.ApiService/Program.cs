var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.AddQdrantClient("retail-app-days");

var app = builder.Build();

app.UseExceptionHandler();
app.MapGet("/weatherforecast", () =>
{
});

app.MapDefaultEndpoints();
app.Run();
