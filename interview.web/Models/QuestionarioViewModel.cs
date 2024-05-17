﻿namespace interview.web.Models
{
    public class QuestionarioViewModel
    {
        public Guid? Id { get; set; }
        public string? Nome { get; set; } = default!;
        public DateTime? DataCriacao { get; set; }
        public int? AvaliacoesRespondidas { get; set; }
        public ICollection<PerguntaQuestionarioViewModelAvaliador> Perguntas { get; set; } = default!;

    }

    public class PerguntaQuestionarioViewModelAvaliador
    {
        public string Id { get; set; }
    }

    public class AlternativaPerguntaQuestionarioViewModelAvaliador
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = default!;
        public bool Correta { get; set; }
    }

    public class AdicionarQuestionarioViewModel
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = default!;
        public ICollection<PerguntaQuestionarioViewModelAvaliador> Perguntas { get; set; }
    }
}