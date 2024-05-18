using interview.web.Models.Enums;

namespace interview.web.Models
{
    public class UsuarioViewModel
    {
        public Guid? id { get; set; }
        public string? cpf { get; set; }
        public string? nome { get; set; }
        public Perfil perfil { get; set; }
        public string? login { get; set; }
        public string? senha { get; set; }
    }

    public class UsuarioViewResponseModel
    {
        public Guid? id { get; set; }
        public string? cpf { get; set; }
        public string? nome { get; set; }
        public Perfil perfil { get; set; }
        public string? login { get; set; }
    }
}
