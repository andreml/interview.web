namespace interview.web.Models
{
    public class LoginViewModel
    {
        public string login { get; set; }
        public string senha { get; set; }
    }

    public class LoginViewResponseModel
    {
        public string nome { get; set; }
        public string perfil { get; set; }
        public string token { get; set; }
    }
}