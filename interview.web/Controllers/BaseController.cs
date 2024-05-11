using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace interview.web.Controllers
{
    public class BaseController : Controller
    {
        public IHttpClientFactory httpClient;
        public BaseController(IHttpClientFactory http)
        {
            httpClient = http;
        }

        public BaseController()
        {
        }

        public string GetToken(IMemoryCache cache)
        {
            string? token = cache.Get("token")?.ToString();
            if (token is null)
                throw new Exception("Sessão expirou");
            return token;
        }

    }
}
