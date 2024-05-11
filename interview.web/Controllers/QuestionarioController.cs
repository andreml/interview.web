using Microsoft.AspNetCore.Mvc;

namespace interview.web.Controllers
{
    public class QuestionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
