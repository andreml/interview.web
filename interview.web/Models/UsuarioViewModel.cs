namespace interview.web.Models
{
    public class UsuarioViewModel
    {
        public string? cpf { get; set; }
        public string? nome { get; set; }
        public string? perfil { get; set; }
        public string? login { get; set; }
        public string? senha { get; set; }
    }

    public class UsuarioViewResponseModel
    {
        public string? id { get; set; }
        public string? cpf { get; set; }
        public string? nome { get; set; }
        public string? perfil { get; set; }
        public string? login { get; set; }
    }
}
