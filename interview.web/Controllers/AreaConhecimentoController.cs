using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class AreaConhecimentoController : BaseController
    {
        private readonly IGetServices<List<AreaConhecimentoViewModel>> _get;
        private readonly IPostServices<string> _post;
        private readonly IPutServices<string> _put;
        private readonly AppConfig _config;
        public AreaConhecimentoController(IGetServices<List<AreaConhecimentoViewModel>> get,
                                 IPostServices<string> post,
                                 IOptions<AppConfig> options,
                                 IPutServices<string> put)
        {
            _get = get;
            _post = post;
            _config = options.Value;
            _put = put;
        }
        public async Task<IActionResult> Index([FromServices] IMemoryCache cache)
        {
            try
            {
                var token = base.GetToken(cache);
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

        public async Task<ActionResult> Details(Guid id, [FromServices] IMemoryCache cache)
        {
            try
            {
                var response = await GetAreaConhecimento(id, cache);
                return PartialView("_Edit", response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
            
        }

        

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AreaConhecimentoViewModel areaConhecimentoViewModel, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = base.GetToken(cache);
                string url = $"{_config.Url}AreaConhecimento";
                var response = await _put.PutCustomAsync(areaConhecimentoViewModel, url, token);
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Success, response);
                var updated = GetAreaConhecimento(areaConhecimentoViewModel.id.Value, cache);
                return PartialView("_Edit", updated.Result);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task<AreaConhecimentoViewModel> GetAreaConhecimento(Guid id, IMemoryCache cache)
        {
            var token = base.GetToken(cache);
            string url = $"{_config.Url}AreaConhecimento";
            var parameters = new Dictionary<string, object>
                {
                    { "areaConhecimentoId", id }
                };
            var response = await _get.GetCustomQueryIdAsync(url, token, parameters);
            return response[0];
        }
    }
}
