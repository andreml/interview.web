namespace interview.web.Models.Dto;

public class AdicionarQuestionarioDto
{
    public string Nome { get; set; } = default!;
    public List<Guid> Perguntas { get; set; } = default!;
}
