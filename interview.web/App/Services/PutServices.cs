using interview.web.App.Interfaces;
using interview.web.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Text;

namespace interview.web.App.Services
{
    public class PutServices<T> : IPutServices<T> where T : class
    {
        readonly IHttpClientFactory _http;
        public PutServices(IHttpClientFactory http)
        {
            _http = http;
        }
        public async Task<T> PutCustomAsync(object body, string url, string token)
        {
            try
            {
                if (body is null || token is null || url is null) throw new ArgumentException("Campos obrigatórios não informados (url, body, token)");

                using (var client = _http.CreateClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    var stringContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                    var response = client.PutAsync(url, stringContent).Result;
                    var stringResponse = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var retorno = (T)Convert.ChangeType(stringResponse, typeof(T));
                        return await Task.FromResult(retorno);
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
