using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models.Dto;
using interview.web.Models.Response;
using interview.web.Models.View;
using interview.web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers;

public class AvaliacaoController : BaseController
{
    private readonly AppConfig _config;
    private readonly IGetServices<GetAvaliacoesEnviadasResponse> _getAvaliacoesQuestionario;
    private readonly IGetServices<GetDetalheRespostasAvaliacaoResponse> _getDetalhesAvaliacao;
    private readonly IGetServices<List<AvaliacaoCandidatoViewModel>> _getAvaliacoesCandidato;
    private readonly IGetServices<ResponderAvaliacaoViewModel> _getResponderAvaliacao;
    private readonly IPutServices<string> _put;

    public AvaliacaoController(
                    IOptions<AppConfig> options,
                    IGetServices<GetAvaliacoesEnviadasResponse> getAvaliacoesQuestionario,
                    IGetServices<GetDetalheRespostasAvaliacaoResponse> getDetalhesAvaliacao,
                    IGetServices<List<AvaliacaoCandidatoViewModel>> getAvaliacoesCandidato,
                    IGetServices<ResponderAvaliacaoViewModel> getResponderAvaliacao,
                    IPutServices<string> put)
    {
        _config = options.Value;
        _getAvaliacoesQuestionario = getAvaliacoesQuestionario;
        _getDetalhesAvaliacao = getDetalhesAvaliacao;
        _getAvaliacoesCandidato = getAvaliacoesCandidato;
        _getResponderAvaliacao = getResponderAvaliacao;
        _put = put;
    }

    public async Task<IActionResult> ExibirTelaAvaliacoesEnviadas(string id, string nome, [FromServices] IMemoryCache cache)
    {
        var token = GetToken(cache);
        if (token == null)
            return RedirecionarParaLogin();

        var response = await _getAvaliacoesQuestionario.GetCustomAsync($"{_config.Url}Avaliacao/enviadas/{id}", token);

        if (response == null)
        {
            TempData["MensagemQuestionario"] =
                Utility.Utils.ShowAlert(Alerts.Error, "Não há avaliação enviada deste questionário");

            return RedirectToAction("Index", "Questionario");
        }

        response.NomeQuestionario = nome;

        return View("_ListaAvaliacoesEnviadas", response);
    }

    public async Task<IActionResult> IndexCandidato([FromServices] IMemoryCache cache)
    {
        var token = GetToken(cache);
        if (token == null)
            return RedirecionarParaLogin();

        var mensagem = (string)TempData["MensagemAvaliacao"]!;
        if (!string.IsNullOrEmpty(mensagem))
            ViewBag.Alert = mensagem;

        var response = (await _getAvaliacoesCandidato.GetCustomAsync($"{_config.Url}Avaliacao/Candidato", token))
                ?? new List<AvaliacaoCandidatoViewModel>();

        return View("IndexCandidato", response);
    }

    public async Task<IActionResult> ExibirTelaResponderAvaliacao(string id, [FromServices] IMemoryCache cache)
    {
        var token = GetToken(cache);
        if (token == null)
            return RedirecionarParaLogin();

        var response = await _getResponderAvaliacao.GetCustomAsync($"{_config.Url}Avaliacao/ObterParaResponder/{id}", token);

        return View("_ResponderAvaliacao", response);
    }

    public async Task<IActionResult> ExibirRespostasAvaliacao(string idAvaliacao, [FromServices] IMemoryCache cache)
    {
        var token = GetToken(cache);
        if (token == null)
            return RedirecionarParaLogin();

        var response = await _getDetalhesAvaliacao.GetCustomAsync($"{_config.Url}Avaliacao/detalhes/{idAvaliacao}", token);

        if (response == null)
        {
            TempData["MensagemQuestionario"] =
                Utility.Utils.ShowAlert(Alerts.Error, "Avaliação não encontrada");

            return RedirectToAction("Index", "Questionario");
        }

        return View("_DetalhesAvaliacao", response);
    }

    [HttpPost]
    public async Task<IActionResult> ResponderQuestionario(ResponderAvaliacaoViewModel model, [FromServices] IMemoryCache cache)
    {
        var token = GetToken(cache);
        if (token == null)
            return RedirecionarParaLogin();

        var dto = new ResponderAvaliacaoDto
        {
            AvaliacaoId = model.AvaliacaoId,
            Respostas = model.Perguntas.Select(p => new ResponderAvaliacaoResposta
            {
                PerguntaId = p.Id,
                AlternativaId = p.AlternativaSelecionadaId!.Value
            }).ToList()
        };

        await _put.PutCustomAsync(dto, $"{_config.Url}Avaliacao/responder", token);

        TempData["MensagemAvaliacao"] = Utils.ShowAlert(Alerts.Success, "Avaliação respondida");

        return await IndexCandidato(cache);
    }
}
