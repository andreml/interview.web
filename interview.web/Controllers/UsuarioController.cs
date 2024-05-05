using interview.web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace interview.web.Controllers
{
    public class UsuarioController : BaseController
    {
        public IActionResult Index()
        {
            try
            {
                //var client = httpClient.CreateClient();
                //var response = await client.GetAsync("https://techchallenge.azurewebsites.net/" + "Usuario");

                //if (response.IsSuccessStatusCode)
                //{
                //    var obj = await response.Content?.ReadAsStringAsync();
                //    return View(JsonConvert.DeserializeObject<UsuarioViewModel>(obj));
                //}
                //else
                //{
                //    var obj = await response.Content?.ReadAsStringAsync();
                //    ViewBag.Erro = JsonConvert.DeserializeObject<ErrorViewModel>(obj).Erro;
                //    return View();
                //}

                var i = new List<UsuarioViewModel>();
                i.Add(new UsuarioViewModel()
                {
                    Cpf = "99999911199",
                    Login = "andre.lima@skillcheck.com",
                    Nome = "Andre Muniz",
                    Perfil = "Avaliador",
                    Senha = ""
                });

                i.Add(new UsuarioViewModel()
                {
                    Cpf = "13245678977",
                    Login = "andre.muniz@skillcheck.com",
                    Nome = "Andre Lima",
                    Perfil = "Candidato",
                    Senha = ""
                });

                
                return View(i.AsEnumerable());
            }
            catch (Exception e)
            {
                ViewBag.Erro = e.Message;
                return View();
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
        public ActionResult Create(IFormCollection collection)
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
    }
}
