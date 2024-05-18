namespace interview.web.Models.Dto;

public class ResponderAvaliacaoDto
{
    public Guid AvaliacaoId { get; set; }
    public List<ResponderAvaliacaoResposta> Respostas { get; set; } = default!;
}

public class ResponderAvaliacaoResposta
{
    public Guid PerguntaId { get; set; }
    public Guid AlternativaId { get; set; }
}
