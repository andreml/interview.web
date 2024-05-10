using interview.web.App.Interfaces;
using interview.web.Models;
using Newtonsoft.Json;
using System.Text;

namespace interview.web.App.Services
{
    public class PostService<T> : IPostServices<T> where T : class
    {
        IHttpClientFactory _http;
        public PostService(IHttpClientFactory http)
        {
            _http = http;
        }
        public async Task<T> PostCustomAsync(object body, string url, string token)
        {
            try
            {
                if (body is null || token is null || url is null) throw new ArgumentException("Campos obrigatórios não informados (url, body, token)");

                using (var client = _http.CreateClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    var strinContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(url, strinContent).Result;
                    var stringResponse = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var deserializeResult = JsonConvert.DeserializeObject<T>(stringResponse);
                        if (deserializeResult != null) return await Task.FromResult(deserializeResult);
                        else throw new Exception($"Não foi possível deserializar o retorno da API {url}");
                    }
                    else
                    {
                        var erroObj = JsonConvert.DeserializeObject<ErrorViewModel>(stringResponse);
                        string msg = string.Empty;
                        foreach (var item in erroObj?.mensagens!) msg += msg + Environment.NewLine;

                        throw new Exception(msg);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
