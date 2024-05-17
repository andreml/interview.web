namespace interview.web.Models.Dto;

public class EnviarAvaliacaoParaCandidatoDto
{
    public string LoginCandidato { get; set; } = default!;
    public Guid QuestionarioId { get; set; }
}
