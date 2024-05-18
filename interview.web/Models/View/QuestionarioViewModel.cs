namespace interview.web.Models.View;
public class QuestionarioViewModel
{
    public string Id { get; set; } = string.Empty;
    public string NomeQuestionario { get; set; } = string.Empty;
    public List<QuestionarioPerguntaViewModel> Perguntas { get; set; } = new List<QuestionarioPerguntaViewModel>();

    public QuestionarioViewModel()
    {
    }
}

public class QuestionarioPerguntaViewModel
{
    public bool Selecionada { get; set; } = default!;
    public string Descricao { get; set; } = default!;
    public Guid Id { get; set; } = default!;
    public string AreaConhecimento { get; set; } = default!;

    public QuestionarioPerguntaViewModel(PerguntaViewResponseModel pergunta)
    {
        Id = pergunta.Id;
        Descricao = pergunta.Descricao;
        AreaConhecimento = pergunta.AreaConhecimento;
        Selecionada = false;
    }

    public QuestionarioPerguntaViewModel()
    {
    }
}
