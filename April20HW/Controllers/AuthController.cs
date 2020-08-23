using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using April20HW.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace April20HW.Controllers
{
    public class AuthController : Controller
    {
        private string _connection = @"Data Source=.\sqlexpress;Initial Catalog=ComplexAds;Integrated Security=true;";
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(string Name, string Email, string Password)
        {
            var db = new DB(_connection);
            db.SignUp(Name, Email, Password);
            return Redirect("/auth/signup");
        }
        public IActionResult Login()
        {
            if(TempData["error"] != null)
            {
                ViewBag.Message = TempData["error"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new DB(_connection);
            var user = db.LogIn(email, password);
            if(user== null)
            {
                TempData["error"] = "Invalid Login";
                return Redirect("/account/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/home");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/Home/Home");
        }

    }
}
