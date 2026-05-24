using System.Text;
using Newtonsoft.Json;

namespace OllamaRagDemo.Services;

public class OllamaService
{
    private readonly HttpClient _httpClient;

    public OllamaService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> AskAsync(string prompt)
    {
        var request = new
        {
            //phi3:mini
            //tinyllama
            model = "tinyllama",
            prompt = prompt,
            stream = false
        };

        var json = JsonConvert.SerializeObject(request);


        var response = await _httpClient.PostAsync(
            "http://localhost:11434/api/generate",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var result = await response.Content.ReadAsStringAsync();

        dynamic responseObject =
            JsonConvert.DeserializeObject(result);

        return responseObject.response.ToString();
    }
}