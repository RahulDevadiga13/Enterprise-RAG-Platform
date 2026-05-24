using System.Text;
using UglyToad.PdfPig;

namespace PdfRagMapper.Services;

public class PdfService
{
    public string ExtractText(string filePath)
    {
        var text = new StringBuilder();

        using (PdfDocument document = PdfDocument.Open(filePath))
        {
            foreach (var page in document.GetPages())
            {
                text.AppendLine(page.Text);
            }
        }

        return text.ToString();
    }
}