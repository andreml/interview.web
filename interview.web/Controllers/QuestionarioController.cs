using Humanizer;
using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class QuestionarioController : BaseController
    {
        private readonly IGetServices<List<QuestionarioViewModel>> _get;
        private readonly IGetServices<List<PerguntaViewResponseModel>> _getPergunta;
        private readonly IPostServices<string> _post;
        private readonly IPutServices<string> _put;
        private readonly IDeleteServices<string> _delete;
        private readonly AppConfig _config;

        public QuestionarioController(IGetServices<List<QuestionarioViewModel>> get,
                                      IPostServices<string> post,
                                      IPutServices<string> put,
                                      IDeleteServices<string> delete,
                                      IOptions<AppConfig> options,
                                      IGetServices<List<PerguntaViewResponseModel>> getPergunta)
        {
            _get = get;
            _post = post;
            _put = put;
            _delete = delete;
            _getPergunta = getPergunta;
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

            return PartialView("_Create", new AdicionarQuestionarioViewModel() { Perguntas = new List<PerguntaQuestionarioViewModelAvaliador>() });

        }

        public async Task<ActionResult> PerguntaAdicionar(string descricao,
                                                        AdicionarQuestionarioViewModel model,
                                                        [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = base.GetToken(cache);
                string url = $"{_config.Url}Pergunta";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("descricao", descricao);
                var response = await _getPergunta.GetCustomQueryIdAsync(url, token, parameters);

                var tt = new PerguntaQuestionarioViewModelAvaliador();
                tt.Id = response[0].id;
                if (model.Perguntas == null) model.Perguntas = new List<PerguntaQuestionarioViewModelAvaliador>();
                model.Perguntas.Add(tt);
                return PartialView("_Create", model);

            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return PartialView("_Create");
            }

        }

        public async Task<ActionResult> DeletarPergunta(string id, List<PerguntaQuestionarioViewModelAvaliador> lista)
        {
            return Ok();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AdicionarQuestionarioViewModel adicionarQuestionarioViewModel, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = base.GetToken(cache);
                string url = $"{_config.Url}Questionario";
                var response = await _post.PostCustomAsync(adicionarQuestionarioViewModel, url, token);
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Success, response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);

            }
            return RedirectToAction("Index", "Questionario");
        }
    }
}
