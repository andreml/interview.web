namespace interview.web.Models.Dto;

public class AlterarQuestionarioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = default!;
    public List<Guid> Perguntas { get; set; } = default!;
}
