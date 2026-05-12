using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceWeb.Models; 

namespace EcommerceWeb.Controllers
{
    public class AccountController : Controller
    {
        ECommerceDBEntities db = new ECommerceDBEntities();

       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                Session["User"] = user.UserName;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!!";
            return View();
        }
    }
}