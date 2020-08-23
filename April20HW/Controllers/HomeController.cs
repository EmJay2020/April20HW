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

        public IActionResult Home()
        {
            var db = new DB(_connection);
            var vm = new HomeViewModel();
            vm.Ads = db.GetAds();
            if(User.Identity.Name != null)
            {
                vm.User = db.GetByEmail(User.Identity.Name);
            }
           
            return View(vm);
        }
        public IActionResult Delete(int Id)
        {
            var db = new DB(_connection);
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
        public IActionResult Add(Ads ad)
        {
            var db = new DB(_connection);
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
