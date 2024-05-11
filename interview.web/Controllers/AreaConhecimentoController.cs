using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class AreaConhecimentoController : BaseController
    {
        private readonly IGetServices<List<AreaConhecimentoResponseViewModel>> _get;
        private readonly IPostServices<string> _post;
        private readonly AppConfig _config;
        public AreaConhecimentoController(IGetServices<List<AreaConhecimentoResponseViewModel>> get,
                                 IPostServices<string> post,
                                 IOptions<AppConfig> options)
        {
            _get = get;
            _post = post;
            _config = options.Value;
        }
        public async Task<IActionResult> Index([FromServices] IMemoryCache cache)
        {
            try
            {
                var token = this.GetToken(cache);
                string url = _config.Url + "AreaConhecimento";
                var response = await _get.GetCustomAsync(url, token);
                return View(response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        private string GetToken(IMemoryCache cache)
        {
            string? token = cache.Get("token")?.ToString();
            if (token is null) 
                throw new Exception("Sessão expirou");
            return token;
        }
    }
}
