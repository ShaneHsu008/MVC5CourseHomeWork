using MVC5CourseHomeWork.Models;
using MVC5CourseHomeWork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5CourseHomeWork.Controllers
{
    public class AccountController : Controller
    {
        private 客戶資料Repository repo;

        public AccountController()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userData = "Admin";

            if (model.帳號 == "9999" && model.密碼 == "admin")
            {
                CreateTicketCookie(model.帳號, userData);
                return RedirectToAction("Index", "Home");
            }

            if (!this.repo.CheckUser(model))
            {
                ModelState.AddModelError(string.Empty, "使用者名稱或密碼錯誤");
                return View(model);
            }

            userData = "一般使用者";
            CreateTicketCookie(model.帳號,userData);

            return RedirectToAction("Edit", "Users");
        }

        private void CreateTicketCookie(string account, string userData)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
              account,
              DateTime.Now,
              DateTime.Now.AddMinutes(30),
              true,
              userData,
              FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}