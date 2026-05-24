using Microsoft.AspNetCore.Mvc;
using PdfRagMapper.Services;

namespace PdfRagMapper.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly PdfService _pdfService;
    private readonly TextChunkService _chunkService;
    private readonly PdfMemoryService _pdfMemoryService;

    public UploadController(
        PdfService pdfService,
        TextChunkService chunkService,
        PdfMemoryService pdfMemoryService)
    {
        _pdfService = pdfService;
        _chunkService = chunkService;
        _pdfMemoryService = pdfMemoryService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadPdf(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file");

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(
            uploadsFolder,
            file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var extractedText =
            _pdfService.ExtractText(filePath);

        var chunks =
            _chunkService.ChunkText(extractedText);

        _pdfMemoryService.Chunks = chunks;

        return Ok(new
        {
            message = "PDF uploaded successfully",
            totalChunks = chunks.Count
        });
    }
}