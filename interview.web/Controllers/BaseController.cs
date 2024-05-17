using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class BaseController : Controller
    {
        public IHttpClientFactory httpClient;
        public BaseController(IHttpClientFactory http)
        {
            httpClient = http;
        }

        public BaseController()
        {
        }

        public string? GetToken(IMemoryCache cache) =>
            cache.Get("token")?.ToString();

        public IActionResult RedirecionarParaLogin()
        {
            TempData["MensagemLogin"] = Utility.Utils.ShowAlert(Alerts.Error, "Realize o login novamente");

            return RedirectToAction("Index", "/");
        }
    }
}
