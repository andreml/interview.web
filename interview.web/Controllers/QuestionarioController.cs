using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using interview.web.Models.Dto;
using interview.web.Models.Response;
using interview.web.Models.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers;

public class QuestionarioController : BaseController
{
    private readonly IGetServices<List<GetQuestionarioResponse>> _get;
    private readonly IGetServices<List<PerguntaViewResponseModel>> _getPergunta;
    private readonly IPostServices<string> _post;
    private readonly IPutServices<string> _put;
    private readonly IDeleteServices<string> _delete;
    private readonly AppConfig _config;

    public QuestionarioController(IGetServices<List<GetQuestionarioResponse>> get,
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
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            var mensagem = (string)TempData["MensagemQuestionario"]!;
            if (!string.IsNullOrEmpty(mensagem))
                ViewBag.Alert = mensagem;

            string url = _config.Url + "Questionario";
            var response = await _get.GetCustomAsync(url, token);
            return response == null ? View(new List<GetQuestionarioResponse>()) : View(response);
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

        var perguntas = await _getPergunta.GetCustomAsync($"{_config.Url}Pergunta", token);

        if (perguntas == null)
        {
            TempData["MensagemQuestionario"] =
                Utility.Utils.ShowAlert(Alerts.Error, "Não há nenhuma pergunta cadastrada para criar um questionário");

            return RedirectToAction("Index");
        }

        var adicionarQuestionarioModel = new QuestionarioViewModel
        {
            Id = id,
            NomeQuestionario = string.Empty,
            Perguntas = perguntas
                        .OrderBy(p => p.AreaConhecimento)
                        .ThenBy(p => p.Descricao)
                        .Select(p => new QuestionarioPerguntaViewModel(p))
                        .ToList()
        };

        if (!string.IsNullOrEmpty(id))
        {
            var parameters = new Dictionary<string, object> { { "questionarioId", id } };
            var questionarios = await _get.GetCustomQueryIdAsync($"{_config.Url}Questionario", token, parameters);

            if(questionarios == null || !questionarios.Any())
            {
                TempData["MensagemQuestionario"] =
                    Utility.Utils.ShowAlert(Alerts.Error, "Erro ao carregar Questionário para edição");

                return RedirectToAction("Index");
            }

            var questionario = questionarios.First();

            adicionarQuestionarioModel.NomeQuestionario = questionario.Nome!;

            foreach (var pergunta in adicionarQuestionarioModel.Perguntas)
                pergunta.Selecionada = questionario.Perguntas.Select(p => p.Id).Contains(pergunta.Id);
        }

        return View("_CreateEdit", adicionarQuestionarioModel);
    }

    public async Task<IActionResult> Delete(string id, [FromServices] IMemoryCache cache)
    {
        try
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            var response = await _delete.DeleteByIdCustomAsync($"{_config.Url}Questionario", token, id);

            TempData["MensagemQuestionario"] =
                Utility.Utils.ShowAlert(Alerts.Success, "Questionário excluído");
        }
        catch (Exception e)
        {
            TempData["MensagemQuestionario"] = Utility.Utils.ShowAlert(Alerts.Error, e.Message);
        }
        return RedirectToAction("Index", "Questionario");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdicionarAlterarQuestionario(QuestionarioViewModel adicionarQuestionario, [FromServices] IMemoryCache cache)
    {
        try
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            //Criacao
            if (string.IsNullOrEmpty(adicionarQuestionario.Id))
            {
                var dto = new AdicionarQuestionarioDto
                {
                    Nome = adicionarQuestionario.NomeQuestionario,
                    Perguntas = adicionarQuestionario
                                            .Perguntas
                                            .Where(p => p.Selecionada)
                                            .Select(p => p.Id)
                                            .ToList()

                };

                await _post.PostCustomAsync(dto, $"{_config.Url}Questionario", token);

                TempData["MensagemQuestionario"] =
                    Utility.Utils.ShowAlert(Alerts.Success, "Questionário adicionado com sucesso");
            }
            //Alteracao
            else
            {
                var dto = new AlterarQuestionarioDto
                {
                    Id = Guid.Parse(adicionarQuestionario.Id),
                    Nome = adicionarQuestionario.NomeQuestionario,
                    Perguntas = adicionarQuestionario
                                            .Perguntas
                                            .Where(p => p.Selecionada)
                                            .Select(p => p.Id)
                                            .ToList()
                };

                await _put.PutCustomAsync(dto, $"{_config.Url}Questionario", token);

                TempData["MensagemQuestionario"] =
                    Utility.Utils.ShowAlert(Alerts.Success, "Questionário alterado com sucesso");
            }
        }
        catch (Exception e)
        {
            TempData["MensagemQuestionario"] =
                Utility.Utils.ShowAlert(Alerts.Error, e.Message);
        }

        return RedirectToAction("Index", "Questionario");
    }

    public IActionResult ExibirTelaEnviarParaCandidato(string id, [FromServices] IMemoryCache cache)
    {
        var token = GetToken(cache);
        if (token == null)
            return RedirecionarParaLogin();

        var viewModel = new EnviarQuestionarioCandidatoViewModel
        {
            QuestionarioId = Guid.Parse(id)
        };

        return View("_SendCandidate", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnviarQuestionarioCandidato(EnviarQuestionarioCandidatoViewModel model, [FromServices] IMemoryCache cache)
    {
        try
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            var dto = new EnviarAvaliacaoParaCandidatoDto
            {
                LoginCandidato = model.LoginCandidato,
                QuestionarioId = model.QuestionarioId,
            };

            await _post.PostCustomAsync(dto, $"{_config.Url}Avaliacao/enviarParaCandidato", token);

            TempData["MensagemQuestionario"] =
                Utility.Utils.ShowAlert(Alerts.Success, "Questionário enviado com sucesso");

            return RedirectToAction("Index");

        }
        catch(Exception ex)
        {
            ViewBag.Alert = Utility.Utils.ShowAlert(Alerts.Error, ex.Message);

            var viewModel = new EnviarQuestionarioCandidatoViewModel
            {
                QuestionarioId = model.QuestionarioId,
                LoginCandidato = model.LoginCandidato
            };

            return View("_SendCandidate", viewModel);
        }
    }
}