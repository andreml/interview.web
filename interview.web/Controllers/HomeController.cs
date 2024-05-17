using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace interview.web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IGetServices<DashViewModel> _get;
        private readonly ILogger<HomeController> _logger;
        private readonly AppConfig _config;

        public HomeController(
                    IGetServices<DashViewModel> get, 
                    ILogger<HomeController> logger,
                    IOptions<AppConfig> options)
        {
            _get = get;
            _logger = logger;
            _config = options.Value;
        }

        public async Task<IActionResult> Index([FromServices] IMemoryCache cache)
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            string url = _config.Url + "Dash";

            var response = (await _get.GetCustomAsync(url, token)) ?? new DashViewModel();

            return View(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { Erro = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
