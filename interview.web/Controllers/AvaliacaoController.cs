using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models.Response;
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

    public AvaliacaoController(
                    IOptions<AppConfig> options, 
                    IGetServices<GetAvaliacoesEnviadasResponse> getAvaliacoesQuestionario,
                    IGetServices<GetDetalheRespostasAvaliacaoResponse> getDetalhesAvaliacao)
    {
        _config = options.Value;
        _getAvaliacoesQuestionario = getAvaliacoesQuestionario;
        _getDetalhesAvaliacao = getDetalhesAvaliacao;
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
}
