using Microsoft.SemanticKernel;
using PdfRagMapper.VectorDb;

namespace PdfRagMapper.Services;

public class RagService
{
    private readonly EmbeddingService _embeddingService;
    private readonly QdrantService _qdrantService;
    private readonly Kernel _kernel;

    public RagService(
        EmbeddingService embeddingService,
        QdrantService qdrantService,
        Kernel kernel)
    {
        _embeddingService = embeddingService;
        _qdrantService = qdrantService;
        _kernel = kernel;
    }

    public async Task<string> AskQuestionAsync(string question)
    {
        var questionEmbedding =
            await _embeddingService.GenerateEmbeddingAsync(question);

        var relevantChunks =
            await _qdrantService.SearchAsync(questionEmbedding);

        var context = string.Join("\n", relevantChunks);

        var prompt = $@"
You are an intelligent mapping assistant.

Use the context below to answer.

Context:
{context}

Question:
{question}

Rules:
- Return JSON only
- No explanation
- Return structured mappings
";

        var result = await _kernel.InvokePromptAsync(prompt);

        return result.ToString();
    }
}