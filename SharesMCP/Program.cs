using SharesMCP.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Register services
builder.Services.AddHttpClient<CseDataService>();

// Add MCP Server with HTTP transport
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// Map MCP endpoint at root
app.MapMcp();

app.Run();