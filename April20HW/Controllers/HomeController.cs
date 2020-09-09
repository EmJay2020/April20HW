using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using April20HW.Models;
using Microsoft.AspNetCore.Authorization;

namespace April20HW.Controllers
{
    public class HomeController : Controller
    {
        private string _connection = @"Data Source=.\sqlexpress;Initial Catalog=ComplexAds;Integrated Security=true;";

        public IActionResult Home(int page)
        {
            var db = new DB(_connection);
            var vm = new HomeViewModel();
          
            if(User.Identity.Name != null)
            {
                vm.User = db.GetByEmail(User.Identity.Name);
            }
            if(TempData["Hacker"] != null)
            {
                ViewBag.message = TempData["Hacker"];
            }
            int pagecount = 5;
            if(page <= 0)
            {
                page = 1;
            }
            int amount = db.GetCount();
            int from = (page - 1) * pagecount;
            int to = from + pagecount;
            if(to < amount)
            {
                vm.Previous = page + 1;
            }
            if(page > 1)
            {
                vm.Next = page - 1;

            }
            vm.Ads = db.GetAds(from, pagecount);

            return View(vm);
        }
        public IActionResult Delete(int Id)
        {
            var db = new DB(_connection);
            var u = db.GetByEmail(User.Identity.Name);
            var uA = db.GetAdsForUser(u);
            bool hack = true;
            foreach(var x in uA)
            {
                if(x.Id == Id)
                {
                    hack = false;                    
                }
            }
            if (hack)
            {
                TempData["Hacker"] = "We are watching you!";
                return Redirect("/home/home");
            }
            db.Delete(Id);
            return Redirect("/home/home");
        }
        [Authorize]
        public IActionResult AddAd()
        {
            var db = new DB(_connection);
            var vm = new AddViewModel();
            vm.User = db.GetByEmail(User.Identity.Name);
            return View(vm);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Add(Ads ad)
        {
            var db = new DB(_connection);
            var a = db.GetByEmail(User.Identity.Name);
            if(ad.UserId != a.Id)
            {
                TempData["Hacker"] = "I know what you are up to";
                return Redirect("/home/AddAd");
            }
            db.AddAd(ad);
            return Redirect("/home/home");

        }
        [Authorize]
        public IActionResult Account()
        {
            var db = new DB(_connection);
            var user = db.GetByEmail(User.Identity.Name);
            return View(db.GetAdsForUser(user));
        }
    }
}
