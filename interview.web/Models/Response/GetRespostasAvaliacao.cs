namespace interview.web.Models.Response;

public class GetDetalheRespostasAvaliacaoResponse
{
    public Guid Id { get; set; }
    public string Candidato { get; set; } = default!;
    public string NomeQuestionario { get; set; } = default!;
    public Guid QuestionarioId { get; set; }
    public string DataEnvio { get; set; } = default!;
    public string DataResposta { get; set; } = default!;
    public string ObservacaoAvaliador { get; set; } = default!;
    public decimal Nota { get; set; }
    public bool Respondido { get; set; }

    public List<DetalheRespostasAvaliacaoResponse> Respostas { get; set; } = default!;
}

public class DetalheRespostasAvaliacaoResponse
{
    public string Pergunta { get; set; } = default!;
    public string RespostaEscolhida { get; set; } = default!;
    public bool Correta { get; set; }
}
