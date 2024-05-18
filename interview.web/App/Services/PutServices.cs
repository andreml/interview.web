using interview.web.App.Interfaces;
using interview.web.Models;
using Newtonsoft.Json;
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
                else if ((int)response.StatusCode == 401 || (int)response.StatusCode == 403)
                {
                    throw new UnauthorizedAccessException("Usuário não tem acesso à este recurso");
                }
                else
                {
                    var erroObj = JsonConvert.DeserializeObject<ErrorViewModel>(stringResponse);

                    var mensagem = "Erro ao processar a requisição";

                    if (erroObj != null && erroObj.mensagens != null && erroObj.mensagens.Any())
                        mensagem = string.Join("; ", erroObj!.mensagens!);

                    throw new BadHttpRequestException(mensagem);
                }
            }

        }
    }
}
