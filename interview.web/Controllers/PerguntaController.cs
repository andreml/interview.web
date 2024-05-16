using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class PerguntaController : BaseController
    {
        private readonly IGetServices<IEnumerable<PerguntaViewResponseModel>> _get;
        private readonly IPostServices<string> _post;
        private readonly IPutServices<string> _put;
        private readonly IDeleteServices<string> _delete;
        private readonly AppConfig _config;
        public PerguntaController(IGetServices<IEnumerable<PerguntaViewResponseModel>> get,
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
        public async Task<IActionResult> Index([FromServices] IMemoryCache cache, string? perguntaId, string? areaConhecimento, string? descricao)
        {
            try
            {
                var response = await ObterPerguntas(cache, perguntaId, areaConhecimento, descricao);
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
        public async Task<IActionResult> Create(IFormCollection collection, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = this.GetToken(cache);
                var url = _config.Url + "Pergunta";
                var body = new PerguntaViewRequestModel() { descricao = collection["descricao"].ToString(), areaConhecimento = collection["areaConhecimento"].ToString(), alternativas = new List<AlternativaRequest>() };

                body.alternativas.Add(new AlternativaRequest() { descricao = collection["resposta1"]!.ToString(), correta = bool.Parse(collection["correta1"]!.First()!.ToString()) });
                body.alternativas.Add(new AlternativaRequest() { descricao = collection["resposta2"]!.ToString(), correta = bool.Parse(collection["correta2"]!.First()!.ToString()) });
                body.alternativas.Add(new AlternativaRequest() { descricao = collection["resposta3"]!.ToString(), correta = bool.Parse(collection["correta3"]!.First()!.ToString()) });
                body.alternativas.Add(new AlternativaRequest() { descricao = collection["resposta4"]!.ToString(), correta = bool.Parse(collection["correta4"]!.First()!.ToString()) });
                body.alternativas.Add(new AlternativaRequest() { descricao = collection["resposta5"]!.ToString(), correta = bool.Parse(collection["correta5"]!.First()!.ToString()) });
                body.alternativas.RemoveAll(p => string.IsNullOrEmpty(p.descricao));

                var response = await _post.PostCustomAsync(body, url, token);
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Success, response);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return View();
            }
        }

        public async Task<IActionResult> Edit([FromServices] IMemoryCache cache, string id)
        {
            try
            {
                var response = this.ObterPerguntas(cache, id, null, null).Result.FirstOrDefault();
                var result = new PerguntaViewModel();
                result.id = id;
                result.descricao = response.descricao;
                result.areaConhecimento = response.areaConhecimento;
                int count = 1;

                for (int i = 0; i < response!.alternativas.Count; i++)
                {
                    switch (count)
                    {
                        case 1:
                            result.resposta1 = response!.alternativas[i].descricao;
                            result.correta1 = response.alternativas[i].correta;
                            break;
                        case 2:
                            result.resposta2 = response!.alternativas[i].descricao;
                            result.correta2 = response.alternativas[i].correta;
                            break;
                        case 3:
                            result.resposta3 = response!.alternativas[i].descricao;
                            result.correta3 = response.alternativas[i].correta;
                            break;
                        case 4:
                            result.resposta4 = response!.alternativas[i].descricao;
                            result.correta4 = response.alternativas[i].correta;
                            break;
                        case 5:
                            result.resposta5 = response!.alternativas[i].descricao;
                            result.correta5 = response.alternativas[i].correta;
                            break;
                        default:
                            break;
                    }

                    count++;
                }

                return View(result);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return View(new PerguntaViewResponseModel() { alternativas = new List<Alternativa>() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection, [FromServices] IMemoryCache cache, string id)
        {
            try
            {
                var token = this.GetToken(cache);
                string url = _config.Url + "Pergunta";

                var body = new PerguntaViewResponseModel() { id = collection["id"].ToString(), descricao = collection["descricao"].ToString(), areaConhecimento = collection["areaConhecimento"].ToString() };
                body.alternativas = new List<Alternativa>();
                body.alternativas.Add(new Alternativa() { descricao = collection["resposta1"]!.ToString(), correta = bool.Parse(collection["correta1"]!.First()!.ToString()) });
                body.alternativas.Add(new Alternativa() { descricao = collection["resposta2"]!.ToString(), correta = bool.Parse(collection["correta2"]!.First()!.ToString()) });
                body.alternativas.Add(new Alternativa() { descricao = collection["resposta3"]!.ToString(), correta = bool.Parse(collection["correta3"]!.First()!.ToString()) });
                body.alternativas.Add(new Alternativa() { descricao = collection["resposta4"]!.ToString(), correta = bool.Parse(collection["correta4"]!.First()!.ToString()) });
                body.alternativas.Add(new Alternativa() { descricao = collection["resposta5"]!.ToString(), correta = bool.Parse(collection["correta5"]!.First()!.ToString()) });
                body.alternativas.RemoveAll(p => string.IsNullOrEmpty(p.descricao));

                var response = await _put.PutCustomAsync(body, url, token);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return View(new PerguntaViewResponseModel() { alternativas = new List<Alternativa>() });
            }
        }

        public async Task<IActionResult> Delete([FromServices] IMemoryCache cache, string id)
        {
            var response = this.ObterPerguntas(cache, id, null, null).Result.FirstOrDefault();
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IFormCollection collection, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = this.GetToken(cache);
                var url = _config.Url + "Pergunta";

                var response = await _delete.DeleteByIdCustomAsync(url, token, collection["id"].ToString());
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Success, response);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return View();
            }
        }

        private async Task<IEnumerable<PerguntaViewResponseModel>> ObterPerguntas(IMemoryCache cache, string? perguntaId, string? areaConhecimento, string? descricao)
        {
            var token = this.GetToken(cache);
            string url = _config.Url + "Pergunta";

            var parametros = new Dictionary<string, object>();
            parametros.Add("perguntaId", perguntaId!);
            parametros.Add("areaConhecimento", areaConhecimento!);
            parametros.Add("descricao", descricao!);

            return (await _get.GetCustomQueryIdAsync(url, token, parametros)) 
                        ?? new List<PerguntaViewResponseModel>();
        }
    }
}
