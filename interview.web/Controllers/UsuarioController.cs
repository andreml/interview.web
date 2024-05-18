using interview.web.App.Interfaces;
using interview.web.Config;
using interview.web.Models;
using interview.web.Models.Enums;
using interview.web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static interview.web.Models.Enums.Enumerator;

namespace interview.web.Controllers
{
    public class UsuarioController : BaseController
    {
        readonly IGetServices<UsuarioViewResponseModel> _get;
        readonly IPostServices<string> _post;
        readonly IPutServices<string> _put;
        readonly AppConfig _config;
        public UsuarioController(IGetServices<UsuarioViewResponseModel> get,
                                 IPostServices<string> post,
                                 IPutServices<string> put,
                                 IOptions<AppConfig> options)
        {
            _get = get;
            _post = post;
            _config = options.Value;
            _put = put;
        }
        public async Task<IActionResult> Index([FromServices] IMemoryCache cache)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();

                UsuarioViewResponseModel? response = await GetUsuario(token);
                return View(response);
            }
            catch (BadHttpRequestException e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task<UsuarioViewResponseModel?> GetUsuario(string token)
        {
            string url = _config.Url + "Usuario";
            var response = await _get.GetCustomAsync(url, token);
            return response;
        }

        public IActionResult Logout([FromServices] IMemoryCache cache)
        {
            cache.Remove("token");
            cache.Remove("perfil");

            return RedirectToAction("Index", "/");
        }

        // GET: UsuarioController/Details/5
        public IActionResult Details([FromServices] IMemoryCache cache)
        {
            var token = GetToken(cache);
            if (token == null)
                return RedirecionarParaLogin();

            var usuario = GetUsuario(token);
            var usuarioEdit = new UsuarioViewModel 
            { 
                id = usuario.Result?.id,
                cpf = usuario.Result.cpf,
                login = usuario.Result.login,
                nome = usuario.Result.nome,
                perfil = usuario.Result.perfil,
                senha = string.Empty
            };

            return PartialView("_Edit", usuarioEdit);
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, [FromServices] IMemoryCache cache)
        {
            try
            {
                var url = _config.Url + "Usuario";
                var body = new UsuarioViewModel()
                {
                    cpf = collection["cpf"].ToString(),
                    login = collection["login"].ToString(),
                    nome = collection["nome"].ToString(),
                    perfil = (Perfil)Enum.Parse(typeof(Perfil), collection["perfil"].ToString()),
                    senha = collection["senha"].ToString()
                };

                var response = await _post.PostCustomAsync(body, url, string.Empty);

                TempData["MensagemLogin"] = Utils.ShowAlert(Alerts.Success, "Usuário adicionado com sucesso! Realize o login para se autenticar");

                return RedirectToAction("Index", "/");
            }
            catch (BadHttpRequestException e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IFormCollection collection, [FromServices] IMemoryCache cache)
        {
            try
            {
                var token = GetToken(cache);
                if (token == null)
                    return RedirecionarParaLogin();
                        
                var url = _config.Url + "Usuario";
                var body = new UsuarioViewModel()
                {
                    id = new Guid(collection["id"].ToString()),
                    cpf = collection["cpf"].ToString(),
                    login = collection["login"].ToString(),
                    nome = collection["nome"].ToString(),
                    senha = collection["senha"].ToString()
                };

                var response = _put.PutCustomAsync(body, url, token);

                TempData["MensagemUsuario"] = Utils.ShowAlert(Alerts.Success, response.Result);

                return RedirectToAction("Index", "Usuario");
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
