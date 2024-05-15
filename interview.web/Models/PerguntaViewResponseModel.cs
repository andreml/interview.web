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
        public string id { get; set; }
        public string areaConhecimento { get; set; }
        public string descricao { get; set; }
        public string resposta1 { get; set; }
        public string resposta2 { get; set; }
        public string resposta3 { get; set; }
        public string resposta4 { get; set; }
        public string resposta5 { get; set; }
        public bool correta1 { get; set; }
        public bool correta2 { get; set; }
        public bool correta3 { get; set; }
        public bool correta4 { get; set; }
        public bool correta5 { get; set; }

    }
}