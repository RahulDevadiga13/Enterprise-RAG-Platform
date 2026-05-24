using Microsoft.AspNetCore.Mvc;

namespace PdfRagMapper.Controllers
{
    public class QueryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
