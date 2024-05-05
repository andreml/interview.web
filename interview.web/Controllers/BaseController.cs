using Microsoft.AspNetCore.Mvc;

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
    }
}
