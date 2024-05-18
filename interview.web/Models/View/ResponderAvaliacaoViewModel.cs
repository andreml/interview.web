namespace interview.web.Models.View;

public class ResponderAvaliacaoViewModel
{
    public Guid QuestionarioId { get; set; }
    public Guid AvaliacaoId { get; set; }
    public string NomeQuestionario { get; set; } = default!;
    public List<PerguntaAvaliacao> Perguntas { get; set; } = default!;
}

public class PerguntaAvaliacao
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = default!;
    public List<AlternativaPerguntaAvaliacao> Alternativas { get; set; } = default!;
    public Guid? AlternativaSelecionadaId { get; set; }
}

public class AlternativaPerguntaAvaliacao
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = default!;
}
