using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace PdfRagMapper.VectorDb;

public class QdrantService
{
    private readonly QdrantClient _client;

    public QdrantService()
    {
        _client = new QdrantClient("localhost", 6334);
    }

    public async Task CreateCollectionAsync()
    {
        await _client.CreateCollectionAsync(
            "pdf-mappings",
            new VectorParams
            {
                Size = 1536,
                Distance = Distance.Cosine
            });
    }

    public async Task InsertEmbeddingAsync(
        Guid id,
        List<float> embedding,
        string chunk)
    {
        await _client.UpsertAsync(
            collectionName: "pdf-mappings",
            points: new List<PointStruct>
            {
                new PointStruct
                {
                    Id = id,
                    Vectors = embedding.ToArray(),
                    Payload =
                    {
                        ["text"] = chunk
                    }
                }
            });
    }

    public async Task<List<string>> SearchAsync(List<float> embedding)
    {
        var result = await _client.SearchAsync(
            collectionName: "pdf-mappings",
            vector: embedding.ToArray(),
            limit: 5);

        return result
            .Select(r => r.Payload["text"].StringValue)
            .ToList();
    }
}