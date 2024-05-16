using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using interview.web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class LoginController : Controller
    {
        IPostServices<LoginViewResponseModel> _postService;
        AppConfig _config;
        public LoginController(IPostServices<LoginViewResponseModel> postService,
                               IOptions<AppConfig> options)
        {
            _postService = postService;
            _config = options.Value;
        }
        public ActionResult Index()
        {
            var mensagem = TempData["MensagemLogin"] as string;

            if (!string.IsNullOrEmpty(mensagem))
                ViewBag.Alert = mensagem;

            return View(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection, [FromServices] IMemoryCache cache)
        {
            try
            {
                var url = _config.Url + "Usuario/Autenticar";
                var body = new LoginViewModel()
                {
                    login = collection["login"].ToString(),
                    senha = collection["senha"].ToString()
                };

                var response = await _postService.PostCustomAsync(body, url, "token");
                var options = new CookieOptions() { MaxAge = TimeSpan.FromHours(1), HttpOnly = true };
                Response.Cookies.Append("token", response.token, options);
                cache.Set<string>("token", response.token);
                return RedirectToAction("Index", "Home", new { token = response.token });
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return View(nameof(Index));
            }
        }
    }
}