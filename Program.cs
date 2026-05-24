using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Services;
using OllamaRagDemo.Services;
using PdfRagMapper.Services;
using PdfRagMapper.VectorDb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<PdfService>();
builder.Services.AddSingleton<TextChunkService>();
builder.Services.AddSingleton<EmbeddingService>();
builder.Services.AddSingleton<QdrantService>();
builder.Services.AddSingleton<RagService>();
builder.Services.AddSingleton<MappingService>();
builder.Services.AddSingleton<OllamaService>();
builder.Services.AddSingleton<PdfMemoryService>();
builder.Services.AddSingleton((serviceProvider) =>
{
    var configuration = builder.Configuration;

    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddOpenAIChatCompletion(
        modelId: configuration["OpenAI:Model"],
        apiKey: configuration["OpenAI:ApiKey"]);

    return kernelBuilder.Build();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();