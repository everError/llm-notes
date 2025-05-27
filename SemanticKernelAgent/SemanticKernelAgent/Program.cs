using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Semantic Kernel µî·Ï
builder.Services.AddSingleton(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();
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
