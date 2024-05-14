using interview.web.App.Interfaces;
using interview.web.Models;
using Newtonsoft.Json;

namespace interview.web.App.Services
{
    public class DeleteServices<T> : IDeleteServices<T> where T : class
    {
        IHttpClientFactory _http;
        public DeleteServices(IHttpClientFactory http)
        {
            _http = http;
        }

        public async Task<T> DeleteByIdCustomAsync(string urlPath, string token, string id)
        {
            try
            {
                if (urlPath is null || token is null) throw new ArgumentException("Campos obrigatórios não informados (url, token");

                using (var client = _http.CreateClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    var response = client.DeleteAsync($"{urlPath}/{id}").Result;
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
