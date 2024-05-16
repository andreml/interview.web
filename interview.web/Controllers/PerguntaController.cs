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
        IGetServices<IEnumerable<PerguntaViewResponseModel>> _get;
        IPostServices<string> _post;
        IPutServices<string> _put;
        AppConfig _config;
        public PerguntaController(IGetServices<IEnumerable<PerguntaViewResponseModel>> get,
                                  IPostServices<string> post,
                                  IPutServices<string> put,
                                  IOptions<AppConfig> options)
        {
            _get = get;
            _post = post;
            _put = put;
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
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            return View();
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

                var request = new PerguntaViewResponseModel() { id = collection["id"].ToString(), descricao = collection["descricao"].ToString(), areaConhecimento = collection["areaConhecimento"].ToString() };
                request.alternativas = new List<Alternativa>();
                //request.alternativas.Add(new Alternativa() { descricao = collection["resposta1"]!.ToString(), correta = bool.Parse(collection["correta1"]!.ToString()) });
                //request.alternativas.Add(new Alternativa() { descricao = collection["resposta2"]!.ToString(), correta = bool.Parse(collection["correta2"]!.ToString()) });
                //request.alternativas.Add(new Alternativa() { descricao = collection["resposta3"]!.ToString(), correta = bool.Parse(collection["correta3"]!.ToString()) });
                //request.alternativas.Add(new Alternativa() { descricao = collection["resposta4"]!.ToString(), correta = bool.Parse(collection["correta4"]!.ToString()) });
                //request.alternativas.Add(new Alternativa() { descricao = collection["resposta5"]!.ToString(), correta = bool.Parse(collection["correta5"]!.ToString()) });

                request.alternativas.Add(new Alternativa() { descricao = collection["resposta1"]!.ToString(), correta = true });
                request.alternativas.Add(new Alternativa() { descricao = collection["resposta2"]!.ToString(), correta = false });
                request.alternativas.Add(new Alternativa() { descricao = collection["resposta3"]!.ToString(), correta = false });
                request.alternativas.Add(new Alternativa() { descricao = collection["resposta4"]!.ToString(), correta = false });
                request.alternativas.Add(new Alternativa() { descricao = collection["resposta5"]!.ToString(), correta = false });
                var body = JsonConvert.SerializeObject(request);
                var response = await _put.PutCustomAsync(request, url, token);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
                return View(new PerguntaViewResponseModel() { alternativas = new List<Alternativa>() });
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
