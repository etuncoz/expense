using System;
using System.Web.Mvc;
using System.Linq;
using ExpenseApp.Data;
using System.Data.Entity;
using ExpenseApp.Engine.Response;
using ExpenseApp.Engine.Request;
using ExpenseApp.Engine.Domain;
using ExpenseApp.Engine.Handlers;
using log4net;

namespace ExpenseApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Login", "Home");
            return Json(UserHandlers.Login(request));
        }

        public ActionResult Logoff()
        {
            if (UserHandlers.Logoff().IsSuccess)
                return RedirectToAction("Login", "Home");
            return RedirectToAction("Login", "Home");
        }
    }
}