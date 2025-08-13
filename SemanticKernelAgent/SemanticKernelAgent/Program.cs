using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
// Create an MCPClient for the GitHub server
await using IMcpClient mcpClient = await McpClientFactory.CreateAsync(new StdioClientTransport(new()
{
    Name = "GitHub",
    Command = "npx",
    Arguments = ["-y", "@modelcontextprotocol/server-github"],
}));
// Retrieve the list of tools available on the GitHub server
var tools = await mcpClient.ListToolsAsync().ConfigureAwait(false);
foreach (var tool in tools)
{
    Console.WriteLine($"{tool.Name}: {tool.Description}");
}
// Semantic Kernel ���
builder.Services.AddSingleton(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();
#pragma warning disable SKEXP0001 // ������ �� �������� �����Ǹ�, ���� ������Ʈ���� ����ǰų� ���ŵ� �� �ֽ��ϴ�. ����Ϸ��� �� ������ ǥ������ �ʽ��ϴ�.
    kernelBuilder.Plugins.AddFromFunctions("GitHub", tools.Select(aiFunction => aiFunction.AsKernelFunction()));
#pragma warning restore SKEXP0001 // ������ �� �������� �����Ǹ�, ���� ������Ʈ���� ����ǰų� ���ŵ� �� �ֽ��ϴ�. ����Ϸ��� �� ������ ǥ������ �ʽ��ϴ�.

    var apiKey = builder.Configuration["OpenAI:ApiKey"] ?? throw new Exception("OpenAI:ApiKey is required");
    kernelBuilder.AddOpenAIChatCompletion("gpt-4o-mini", apiKey);

    return kernelBuilder.Build();
});

//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
