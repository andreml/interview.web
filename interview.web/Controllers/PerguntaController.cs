using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class PerguntaController : BaseController
    {
        IGetServices<IEnumerable<PerguntaViewResponseModel>> _get;
        IPostServices<string> _post;
        AppConfig _config;
        public PerguntaController(IGetServices<IEnumerable<PerguntaViewResponseModel>> get,
                                 IPostServices<string> post,
                                 IOptions<AppConfig> options)
        {
            _get = get;
            _post = post;
            _config = options.Value;
        }
        public async Task<IActionResult> Index([FromServices] IMemoryCache cache, string? perguntaId, string? areaConhecimento, string? descricao)
        {
            try
            {
                var token = this.GetToken(cache);
                string url = _config.Url + "Pergunta";

                var parametros = new Dictionary<string, object>();
                parametros.Add("perguntaId", perguntaId!);
                parametros.Add("areaConhecimento", areaConhecimento!);
                parametros.Add("descricao", descricao!);

                var response = await _get.GetCustomQueryIdAsync(url, token, parametros);
                return View(response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return View(new PerguntaViewResponseModel() { alternativas = new List<Alternativa>() });
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            return View();
        }
    }
}
