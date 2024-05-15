using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class QuestionarioController : BaseController
    {
        private readonly IGetServices<List<QuestionarioViewModel>> _get;
        private readonly IPostServices<string> _post;
        private readonly IPutServices<string> _put;
        private readonly IDeleteServices<string> _delete;
        private readonly AppConfig _config;

        public QuestionarioController(IGetServices<List<QuestionarioViewModel>> get, 
                                      IPostServices<string> post, 
                                      IPutServices<string> put, 
                                      IDeleteServices<string> delete,
                                      IOptions<AppConfig> options)
        {
            _get = get;
            _post = post;
            _put = put;
            _delete = delete;
            _config = options.Value;
        }

        public async Task<IActionResult> Index([FromServices] IMemoryCache cache)
        {
            try
            {
                var token = base.GetToken(cache);
                string url = _config.Url + "Questionario";
                var response = await _get.GetCustomAsync(url, token);
                return response == null ? View(new List<QuestionarioViewModel>()) : View(response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> Create()
        {
            return PartialView("_Create", new QuestionarioViewModel());

        }
    }
}
