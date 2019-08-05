using Microsoft.AspNetCore.Mvc;
using RepositorioSuprimentos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorioSuprimentos.Controllers {
    public class ContaController : Controller {
        public IActionResult Registrar() {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(ContaRegistrarViewModel modelo) {
            if (ModelState.IsValid) {
                // Podemos incluir o usuario
                return RedirectToAction("Index", "Home");
            }
            return View(modelo);
        }
    }
}
