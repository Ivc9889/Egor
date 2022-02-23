using Egor.Models;
using Egor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Egor.Controllers
{
    public class HomeController : Controller
    {
        EgorContext db;
        public HomeController(EgorContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Depts.ToList());
        }

        [HttpGet]
        public IActionResult Dept(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            Dept dept = db.Depts.Find(id);
            ViewBag.Dept = $"{dept.Code} - {dept.Name}";

            DeptViewModel deptViewModel = new DeptViewModel
            {
                Profiles = db.Profiles.Where(p => p.DeptId == dept.Id),
                TypesProgram = db.TypesProgram.ToList(),
                Disciplines = db.Disciplines.ToList()
            };
            return View(deptViewModel);
        }
    }
}