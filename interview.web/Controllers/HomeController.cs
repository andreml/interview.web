using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using interview.web.Models.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace interview.web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IGetServices<DashViewModel> _get;
        private readonly IGetServices<List<AvaliacaoCandidatoViewModel>> _getAvaliacoesCandidato;
        private readonly ILogger<HomeController> _logger;
        private readonly AppConfig _config;

        public HomeController(
                    IGetServices<DashViewModel> get,
                    IGetServices<List<AvaliacaoCandidatoViewModel>> getAvaliacoesCandidato,
                    ILogger<HomeController> logger,
                    IOptions<AppConfig> options)
        {
            _get = get;
            _getAvaliacoesCandidato = getAvaliacoesCandidato;
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

        public async Task<IActionResult> IndexCandidato([FromServices] IMemoryCache cache)
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            var response = (await _getAvaliacoesCandidato.GetCustomAsync($"{_config.Url}Avaliacao/Candidato", token))
                ?? new List<AvaliacaoCandidatoViewModel>();

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
