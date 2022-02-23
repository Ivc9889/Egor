using Egor.Models;
using Egor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Egor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        EgorContext db;
        public HomeController(EgorContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            AdminViewModel adminViewModel = new AdminViewModel
            {
                Depts = db.Depts.ToList(),
                Users = db.Users.ToList(),
            };
            return View(adminViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id, string? modelDelete)
        {
            if (id == null || modelDelete == null) return RedirectToAction("Index");

            return View(db.Depts.Find(id));
        }

        [HttpPost]
        public IActionResult Delete(Dept dept)
        {
            db.Depts.Remove(dept);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
