using Microsoft.AspNetCore.Mvc;
using OllamaRagDemo.Services;
using PdfRagMapper.Services;

namespace PdfRagMapper.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AskPdfController : ControllerBase
{
    private readonly PdfMemoryService _pdfMemoryService;
    private readonly OllamaService _ollamaService;

    public AskPdfController(
        PdfMemoryService pdfMemoryService,
        OllamaService ollamaService)
    {
        _pdfMemoryService = pdfMemoryService;
        _ollamaService = ollamaService;
    }

    [HttpGet]
    public async Task<IActionResult> Ask(string question)
    {
        var context =
            string.Join("\n", _pdfMemoryService.Chunks);

        var prompt = $@"
You are an AI assistant.

Answer ONLY using the PDF context below.

Give short and direct answers.

Do NOT explain extra things.

PDF Context:
{context}

Question:
{question}

Answer:
";

        var response =
            await _ollamaService.AskAsync(prompt);

        return Ok(response);
    }
}