using interview.web.App.Interfaces;
using interview.web.Models;
using Newtonsoft.Json;
using System.Net;

namespace interview.web.App.Services
{
    public class GetService<T> : IGetServices<T> where T : class
    {
        IHttpClientFactory _http;
        public GetService(IHttpClientFactory http)
        {
            _http = http;
        }

        public async Task<T?> GetCustomAsync(string urlPath, string token)
        {
            try
            {
                if (urlPath is null || token is null) throw new ArgumentException("Campos obrigatórios não informados (url, token");

                using (var client = _http.CreateClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    var response = client.GetAsync(urlPath).Result;
                    var stringResponse = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var deserializeResult = JsonConvert.DeserializeObject<T>(stringResponse);

                        if (deserializeResult != null)
                            return await Task.FromResult(deserializeResult);
                        else if (response.StatusCode == HttpStatusCode.NoContent)
                            return null;

                        else throw new Exception($"Não foi possível deserializar o retorno da API {urlPath}");
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

        public async Task<T> GetCustomQueryIdAsync(string urlPath, string token, Dictionary<string, object> queryStringParameters)
        {
            string parametros = "?";

            foreach (var item in queryStringParameters)
            {
                if (item.Value != null)
                    parametros = $"{parametros}{item.Key}={item.Value}";
            }

            string url = parametros.Length > 1 ? urlPath + parametros : urlPath;
            return await GetCustomAsync(url, token);
        }
    }
}
