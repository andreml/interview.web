namespace interview.web.Models.Response;

public class GetAvaliacoesEnviadasResponse
{
    public Guid IdQuestionario { get; set; }
    public decimal? MediaNota { get; set; }
    public string NomeQuestionario { get; set; } = default!;
    public List<AvaliacoesQuestionarioResponse> AvaliacoesPendentes { get; set; } = default!;
    public List<AvaliacoesQuestionarioResponse> AvaliacoesRespondidas { get; set; } = default!;
}

public class AvaliacoesQuestionarioResponse
{
    public Guid IdAvaliacao { get; set; }
    public string NomeCandidato { get; set; } = default!;
    public decimal? Nota { get; set; }
    public DateTime DataEnvio { get; set; }
    public DateTime? DataResposta { get; set; }
}
