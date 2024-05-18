namespace interview.web.Models
{
    public class PerguntaViewResponseModel
    {
        public Guid Id { get; set; }
        public string AreaConhecimento { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public List<Alternativa> Alternativas { get; set; } = default!;
    }
    public class Alternativa
    {
        public string Descricao { get; set; } = default!;
        public bool Correta { get; set; }

        public Alternativa(string descricao, bool correta)
        {
            Descricao = descricao;
            Correta = correta;
        }
    }

    public class PerguntaViewRequestModel
    {
        public string AreaConhecimento { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public List<AlternativaRequest> Alternativas { get; set; } = default!;
    }
    public class AlternativaRequest
    {
        public string Descricao { get; set; }
        public bool Correta { get; set; }

        public AlternativaRequest(string descricao, bool correta)
        {
            Descricao = descricao;
            Correta = correta;
        }
    }

    public class PerguntaViewModel
    {
        public Guid Id { get; set; }
        public string AreaConhecimento { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public string Resposta1 { get; set; } = default!;
        public string Resposta2 { get; set; } = default!;
        public string Resposta3 { get; set; } = default!;
        public string Resposta4 { get; set; } = default!;
        public string Resposta5 { get; set; } = default!;
        public bool Correta1 { get; set; }
        public bool Correta2 { get; set; }
        public bool Correta3 { get; set; }
        public bool Correta4 { get; set; }
        public bool Correta5 { get; set; }

        public PerguntaViewModel()
        {
        }
    }
}