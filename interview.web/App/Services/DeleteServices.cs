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
