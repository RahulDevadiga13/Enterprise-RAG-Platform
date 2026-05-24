using Microsoft.AspNetCore.Mvc;
using OllamaRagDemo.Services;

namespace OllamaRagDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly OllamaService _ollamaService;

    public AiController(OllamaService ollamaService)
    {
        _ollamaService = ollamaService;
    }

    [HttpGet]
    public async Task<IActionResult> Ask(string question)
    {
        var response =
            await _ollamaService.AskAsync(question);

        return Ok(response);
    }
}