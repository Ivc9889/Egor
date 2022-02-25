using Egor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Egor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfileController : Controller
	{
        private IWebHostEnvironment hostingEnvironment { get; }
        EgorContext db;
        public ProfileController(EgorContext context, IWebHostEnvironment hostingEnvironment)
        {
            db = context;
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index(int id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            Dept dept = db.Depts.Find(id);
            ViewBag.Dept = $"{dept.Code} - {dept.Name}";
            ViewBag.DeptId = id;
            return View(db.Profiles.Where(p => p.DeptId == id));
        }

        [HttpGet]
        public IActionResult Create(int? id)
		{
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            ViewBag.DeptId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Profile profile)
        {
            db.Profiles.Add(profile);
            db.SaveChanges();

            TypeProgram typeProgramBase = new TypeProgram
            { 
                Name = "Базовая",
                ProfileId = profile.Id
            };
            TypeProgram typeProgramVariableRequire = new TypeProgram
            {
                Name = "Вариативная(Обязательные дисциплины)",
                ProfileId = profile.Id
            };
            TypeProgram typeProgramVariableChoose = new TypeProgram
            {
                Name = "Вариативная(По выбору)",
                ProfileId = profile.Id
            };
           
            db.TypesProgram.AddRange(typeProgramBase, typeProgramVariableRequire, typeProgramVariableChoose);
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Profile", action = "Index", id = profile.DeptId });
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            if (id == null) return RedirectToRoute("MyArea", new { area = "Admin", controller = "Profile", action = "Index" });

            Profile profileEdit = db.Profiles.Find(id);
            ViewBag.Depts = new SelectList(db.Depts.Where(s => s.Id != profileEdit.DeptId), "Id", "Name");
            ViewBag.DeptId = db.Depts.FirstOrDefault(s => s.Id == profileEdit.DeptId).Id;
            ViewBag.DeptName = db.Depts.FirstOrDefault(s => s.Id == profileEdit.DeptId).Name;
            return View(profileEdit);
        }

        [HttpPost]
        public IActionResult Edit(Profile profile)
        {
            Profile profileUpdate = db.Profiles.Find(profile.Id);
            profileUpdate.Name = profile.Name;
            profileUpdate.DeptId = profile.DeptId;
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Profile", action = "Index", id = profile.DeptId });
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            return View(db.Profiles.Find(id));
        }

        [HttpPost]
        public IActionResult Delete(Profile profile)
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
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Profile", action = "Index", id = profile.DeptId });
        }
    }
}
