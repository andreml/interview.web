using Microsoft.AspNetCore.Mvc;

namespace interview.web.Controllers
{
    public class PerguntaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
