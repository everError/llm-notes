using ChromaDB.Client;
using Docs_Mcp.Data;
using Docs_Mcp.Ollama;
using Docs_Mcp.Services.Document;
using Docs_Mcp.Services.Embedding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMcpServer()
    .WithHttpTransport() // With streamable HTTP
    .WithToolsFromAssembly(); // Add all classes marked with [McpServerToolType]

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
//Ollama
builder.Services.Configure<OllamaSettings>(builder.Configuration.GetSection("OllamaSettings"));
// HttpClient ��� (OllamaEmbeddingService���� ���)
// Ollama BaseUrl�� HttpClient�� �⺻���� �����մϴ�.
builder.Services.AddHttpClient<IEmbeddingService, OllamaEmbeddingService>(client =>
{
    var ollamaSettings = builder.Configuration.GetSection("OllamaSettings").Get<OllamaSettings>();
    client.BaseAddress = new Uri(ollamaSettings!.BaseUrl);
});

// IEmbeddingService �������̽��� OllamaEmbeddingService ����ü�� ����մϴ�.
// AddHttpClient�� ��������Ƿ� �ڵ����� ��ϵ˴ϴ�.
// builder.Services.AddSingleton<IEmbeddingService, OllamaEmbeddingService>(); // �� ���� �ʿ� ����
//
//ChromaDB
builder.Services.Configure<ChromaDbSettings>(builder.Configuration.GetSection("ChromaDb"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ChromaClient>(sp =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ChromaDbSettings>>().Value;
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();

    var configOptions = new ChromaConfigurationOptions(uri: settings.GetBaseUri());
    return new ChromaClient(configOptions, httpClient);
});
builder.Services.AddScoped<DocumentIngestionService>();
//
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapMcp("/mcp");
app.MapControllers();
app.UseCors();

app.Run();
