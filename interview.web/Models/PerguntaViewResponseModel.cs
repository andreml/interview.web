namespace interview.web.Models
{
    public class PerguntaViewResponseModel
    {
        public string id { get; set; }
        public string areaConhecimento { get; set; }
        public string descricao { get; set; }
        public List<Alternativa> alternativas { get; set; }
    }
    public class Alternativa
    {
        public string id { get; set; }
        public string descricao { get; set; }
        public bool correta { get; set; }
    }

    public class PerguntaViewModel
    {
        public string areaConhecimento { get; set; }
        public string descricao { get; set; }
        public string resposta1 { get; set; }
        public bool selecionado1 { get; set; }
        public string resposta2 { get; set; }
        public bool selecionado2 { get; set; }
        public string resposta3 { get; set; }
        public bool selecionado3 { get; set; }
        public string resposta4 { get; set; }
        public bool selecionado4 { get; set; }
        public string resposta5 { get; set; }
        public bool selecionado5 { get; set; }
        public string correta { get; set; }
    }
}