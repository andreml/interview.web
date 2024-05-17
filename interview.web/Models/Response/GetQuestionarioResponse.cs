namespace interview.web.Models.Response;

public class GetQuestionarioResponse
{
    public Guid? Id { get; set; }
    public string? Nome { get; set; } = default!;
    public DateTime? DataCriacao { get; set; }
    public int? AvaliacoesEnviadas { get; set; }
    public ICollection<GetQuestionarioPerguntaResponse> Perguntas { get; set; } = default!;

}

public class GetQuestionarioPerguntaResponse
{
    public Guid Id { get; set; }
}
