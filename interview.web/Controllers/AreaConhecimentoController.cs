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
        private readonly IGetServices<List<AreaConhecimentoViewModel>> _get;
        private readonly IPostServices<string> _post;
        private readonly IPutServices<string> _put;
        private readonly IDeleteServices<string> _delete;
        private readonly AppConfig _config;

        public AreaConhecimentoController(
                        IGetServices<List<AreaConhecimentoViewModel>> get,
                        IPostServices<string> post,
                        IOptions<AppConfig> options,
                        IPutServices<string> put,
                        IDeleteServices<string> delete)
        {
            _get = get;
            _post = post;
            _config = options.Value;
            _put = put;
            _delete = delete;
        }

        public async Task<IActionResult> Index([FromServices] IMemoryCache cache)
        {
            try
            {
                var mensagem = (string)TempData["MensagemAreaConhecimento"]!;
                if (!string.IsNullOrEmpty(mensagem))
                    ViewBag.Alert = mensagem;

                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                string url = _config.Url + "AreaConhecimento";
                var response = (await _get.GetCustomAsync(url, token)) ?? new List<AreaConhecimentoViewModel>();
                return View(response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ExibirTelaCriarAlterar(string id, [FromServices] IMemoryCache cache)
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            AreaConhecimentoViewModel areaConhecimento;

            if (!string.IsNullOrEmpty(id))
            {
                areaConhecimento = await GetAreaConhecimento(Guid.Parse(id), token);
            }
            else
            {
                areaConhecimento = new AreaConhecimentoViewModel
                {
                    descricao = string.Empty
                };
            }

            return PartialView("_CreateEdit", areaConhecimento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarAlterarQuestionario(AreaConhecimentoViewModel areaConhecimentoViewModel, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                //Criacao
                if (areaConhecimentoViewModel.id == null || areaConhecimentoViewModel.id == Guid.Empty)
                {
                    await _post.PostCustomAsync(areaConhecimentoViewModel, $"{_config.Url}AreaConhecimento", token);

                    TempData["MensagemAreaConhecimento"] =
                        Utility.Utils.ShowAlert(Alerts.Success, "Area de Conhecimento adicionada");
                }
                //Alteracao
                else
                {
                    await _put.PutCustomAsync(areaConhecimentoViewModel, $"{_config.Url}AreaConhecimento", token);

                    TempData["MensagemAreaConhecimento"] = 
                        Utility.Utils.ShowAlert(Alerts.Success, "Area de Conhecimento alterada");
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Delete(string id, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                var response = await _delete.DeleteByIdCustomAsync($"{_config.Url}AreaConhecimento", token, id);

                TempData["MensagemAreaConhecimento"] =
                    Utility.Utils.ShowAlert(Alerts.Success, "Area de Conhecimento excluída");
            }
            catch (Exception e)
            {
                TempData["MensagemAreaConhecimento"] = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
            }

            return RedirectToAction("Index");
        }

        private async Task<AreaConhecimentoViewModel> GetAreaConhecimento(Guid id, string token)
        {
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
