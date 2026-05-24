using PdfRagMapper.VectorDb;

namespace PdfRagMapper.Services;

public class MappingService
{
    private readonly PdfService _pdfService;
    private readonly TextChunkService _chunkService;
    private readonly EmbeddingService _embeddingService;
    private readonly QdrantService _qdrantService;

    public MappingService(
        PdfService pdfService,
        TextChunkService chunkService,
        EmbeddingService embeddingService,
        QdrantService qdrantService)
    {
        _pdfService = pdfService;
        _chunkService = chunkService;
        _embeddingService = embeddingService;
        _qdrantService = qdrantService;
    }

    public async Task ProcessPdfAsync(string filePath)
    {
        var text = _pdfService.ExtractText(filePath);

        var chunks = _chunkService.ChunkText(text);

        foreach (var chunk in chunks)
        {
            var embedding =
                await _embeddingService.GenerateEmbeddingAsync(chunk);

            await _qdrantService.InsertEmbeddingAsync(
                Guid.NewGuid(),
                embedding,
                chunk);
        }
    }
}