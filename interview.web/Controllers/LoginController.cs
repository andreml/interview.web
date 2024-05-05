using Microsoft.AspNetCore.Mvc;

namespace interview.web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(IFormCollection collection)
        {
            try
            {
                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }
    }
}