using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace PdfRagMapper.Services;

public class EmbeddingService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public EmbeddingService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<List<float>> GenerateEmbeddingAsync(string text)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        var model = _configuration["OpenAI:EmbeddingModel"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var request = new
        {
            input = text,
            model = model
        };

        var json = JsonConvert.SerializeObject(request);

        var response = await _httpClient.PostAsync(
            "https://api.openai.com/v1/embeddings",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var result = await response.Content.ReadAsStringAsync();

        dynamic embeddingResult = JsonConvert.DeserializeObject(result);

        return ((IEnumerable<dynamic>)embeddingResult.data[0].embedding)
            .Select(x => (float)x)
            .ToList();
    }
}