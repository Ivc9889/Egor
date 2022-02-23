using Egor.Models;
using Microsoft.AspNetCore.Mvc;

namespace Egor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DeptController : Controller
	{
        private IWebHostEnvironment hostingEnvironment { get; }
        EgorContext db;
        public DeptController(EgorContext context, IWebHostEnvironment hostingEnvironment)
        {
            db = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) return RedirectToRoute("MyArea", new { area = "Admin", controller = "Home", action = "Index" });
            return View(db.Depts.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(Dept dept)
        {          
            Dept deptUpdate = db.Depts.Find(dept.Id);
            deptUpdate.Code = dept.Code;
            deptUpdate.Name = dept.Name;
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Home", action = "Index" });
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return RedirectToRoute("MyArea", new { area = "Admin", controller = "Home", action = "Index" });
            return View(db.Depts.Find(id));
        }

        [HttpPost]
        public IActionResult Delete(Dept dept)
        {
            foreach (Profile profile in db.Profiles.Where(s => s.DeptId == dept.Id))
            {
                foreach (TypeProgram typeProgram in db.TypesProgram.Where(s => s.ProfileId == profile.Id))
                {
                    foreach (Discipline discipline in db.Disciplines.Where(s => s.TypeProgramId == typeProgram.Id))
                    {
                        var dir = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "files", discipline.Content));
                        dir.Delete();
                        db.Disciplines.Remove(discipline);
                    }
                    db.TypesProgram.Remove(typeProgram);
                }
                db.Profiles.Remove(profile);
            }
            db.Depts.Remove(dept);
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Home", action = "Index" });
        }

        [HttpGet]
        public IActionResult Create()
		{
            return View();
		}

        [HttpPost]
        public IActionResult Create(Dept dept)
		{
            db.Depts.Add(dept);
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Home", action = "Index" });
        }
    }
}
