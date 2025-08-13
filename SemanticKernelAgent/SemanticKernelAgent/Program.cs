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
// Semantic Kernel 등록
builder.Services.AddSingleton(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();
#pragma warning disable SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
    kernelBuilder.Plugins.AddFromFunctions("GitHub", tools.Select(aiFunction => aiFunction.AsKernelFunction()));
#pragma warning restore SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.

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
