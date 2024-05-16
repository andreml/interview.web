﻿using interview.web.App.Interfaces;
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
        IGetServices<UsuarioViewResponseModel> _get;
        IPostServices<string> _post;
        AppConfig _config;
        public UsuarioController(IGetServices<UsuarioViewResponseModel> get,
                                 IPostServices<string> post,
                                 IOptions<AppConfig> options)
        {
            _get = get;
            _post = post;
            _config = options.Value;
        }
        public async Task<IActionResult> Index([FromServices] IMemoryCache cache)
        {
            try
            {
                var token = this.GetToken(cache);
                string url = _config.Url + "Usuario";
                var response = await _get.GetCustomAsync(url, token);
                return View(response);
            }
            catch (Exception e)
            {
                ViewBag.Alert = Utils.ShowAlert(Alerts.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return PartialView("_Edit");
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
            catch (Exception e)
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
        public ActionResult Edit(int id, IFormCollection collection)
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

        private string GetToken(IMemoryCache cache)
        {
            string? token = cache.Get("token")?.ToString();
            if (token is null) throw new Exception("Sessão expirou");
            return token;
        }
    }
}
