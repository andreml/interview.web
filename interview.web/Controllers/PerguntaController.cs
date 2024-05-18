using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using interview.web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                var mensagem = (string)TempData["MensagemPergunta"]!;
                if (!string.IsNullOrEmpty(mensagem))
                    ViewBag.Alert = mensagem;

                var response = await ObterPerguntas(token, perguntaId, areaConhecimento, descricao);
                return View(response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return View(new PerguntaViewResponseModel() { Alternativas = new List<Alternativa>() });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                var url = _config.Url + "Pergunta";
                var body = new PerguntaViewRequestModel
                {
                    Descricao = collection["descricao"].ToString(),
                    AreaConhecimento = collection["areaConhecimento"].ToString(),
                    Alternativas = new List<AlternativaRequest>()
                    {
                        new AlternativaRequest(collection["resposta1"]!, false),
                        new AlternativaRequest(collection["resposta2"]!, false),
                        new AlternativaRequest(collection["resposta3"]!, false),
                        new AlternativaRequest(collection["resposta4"]!, false),
                        new AlternativaRequest(collection["resposta5"]!, false),
                    }
                };

                int alternativaCorreta = int.Parse(collection["radio-correta"]!);
                body.Alternativas[alternativaCorreta - 1].Correta = true;

                body.Alternativas.RemoveAll(p => string.IsNullOrEmpty(p.Descricao) && !p.Correta);

                await _post.PostCustomAsync(body, url, token);

                TempData["MensagemPergunta"] =
                        Utils.ShowAlert(Alerts.Success, "Pergunta adicionada com sucesso");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return View();
            }
        }

        public async Task<IActionResult> Edit([FromServices] IMemoryCache cache, string id)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                var response = (await ObterPerguntas(token, id, null, null)).FirstOrDefault();

                var result = new PerguntaViewModel()
                {
                    Id = Guid.Parse(id),
                    Descricao = response!.Descricao,
                    AreaConhecimento = response.AreaConhecimento
                };

                int count = 1;
                for (int i = 0; i < response!.Alternativas.Count; i++)
                {
                    switch (count)
                    {
                        case 1:
                            result.Resposta1 = response!.Alternativas[i].Descricao;
                            result.Correta1 = response.Alternativas[i].Correta;
                            break;
                        case 2:
                            result.Resposta2 = response!.Alternativas[i].Descricao;
                            result.Correta2 = response.Alternativas[i].Correta;
                            break;
                        case 3:
                            result.Resposta3 = response!.Alternativas[i].Descricao;
                            result.Correta3 = response.Alternativas[i].Correta;
                            break;
                        case 4:
                            result.Resposta4 = response!.Alternativas[i].Descricao;
                            result.Correta4 = response.Alternativas[i].Correta;
                            break;
                        case 5:
                            result.Resposta5 = response!.Alternativas[i].Descricao;
                            result.Correta5 = response.Alternativas[i].Correta;
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
                return View(new PerguntaViewResponseModel() { Alternativas = new List<Alternativa>() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection collection, [FromServices] IMemoryCache cache, string id)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                string url = _config.Url + "Pergunta";

                var body = new PerguntaViewResponseModel() 
                { 
                    Id = Guid.Parse(collection["id"]!), 
                    Descricao = collection["descricao"]!, 
                    AreaConhecimento = collection["areaConhecimento"]!,
                    Alternativas = new List<Alternativa>()
                    {
                        new Alternativa(collection["resposta1"]!, false),
                        new Alternativa(collection["resposta2"]!, false),
                        new Alternativa(collection["resposta3"]!, false),
                        new Alternativa(collection["resposta4"]!, false),
                        new Alternativa(collection["resposta5"]!, false),
                    }
                };

                int alternativaCorreta = int.Parse(collection["radio-correta"]!);
                body.Alternativas[alternativaCorreta - 1].Correta = true;

                body.Alternativas.RemoveAll(p => string.IsNullOrEmpty(p.Descricao) && !p.Correta);

                await _put.PutCustomAsync(body, url, token);

                TempData["MensagemPergunta"] = 
                        Utils.ShowAlert(Alerts.Success, "Pergunta alterada com sucesso");

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return View(new PerguntaViewResponseModel() { Alternativas = new List<Alternativa>() });
            }
        }

        public async Task<IActionResult> Delete([FromServices] IMemoryCache cache, string id)
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            var response = (await ObterPerguntas(token, id, null, null)).FirstOrDefault();
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IFormCollection collection, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                var url = _config.Url + "Pergunta";

                var response = await _delete.DeleteByIdCustomAsync(url, token, collection["id"].ToString());

                TempData["MensagemPergunta"] =
                        Utils.ShowAlert(Alerts.Success, "Pergunta excluída com sucesso");

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return View();
            }
        }

        private async Task<IEnumerable<PerguntaViewResponseModel>> ObterPerguntas(string token, string? perguntaId, string? areaConhecimento, string? descricao)
        {
            string url = _config.Url + "Pergunta";

            var parametros = new Dictionary<string, object>()
            {
                {"perguntaId", perguntaId! },
                {"areaConhecimento", areaConhecimento! },
                {"descricao", descricao! },
            };

            return (await _get.GetCustomQueryIdAsync(url, token, parametros)) 
                        ?? new List<PerguntaViewResponseModel>();
        }
    }
}
