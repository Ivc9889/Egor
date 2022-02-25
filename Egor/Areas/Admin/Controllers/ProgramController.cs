using Egor.Models;
using Egor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Egor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProgramController : Controller
    {
        private IWebHostEnvironment hostingEnvironment { get; }
        EgorContext db;
        public ProgramController(EgorContext context, IWebHostEnvironment hostingEnvironment)
        {
            db = context;
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index(int? id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            ShowProgramViewModel showProgramViewModel = new ShowProgramViewModel
            {
                TypesProgram = db.TypesProgram.Where(s => s.ProfileId == id),
                Disciplines = db.Disciplines.ToList()
            };

            Profile profile = db.Profiles.Find(id);
            ViewBag.ProfileId = id;
            ViewBag.ProfileName = profile.Name;
            ViewBag.DeptId = db.Depts.FirstOrDefault(s => s.Id == profile.DeptId).Id;
            return View(showProgramViewModel);
        }

        [HttpGet]
        public IActionResult CreateTypeProgram(int? id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            ViewBag.ProfileId = id;
            return View();
        }

        [HttpPost]
        public IActionResult CreateTypeProgram(TypeProgram typeProgram)
        {
            db.TypesProgram.Add(typeProgram);
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Program", action = "Index", id = typeProgram.ProfileId });
        }

        [HttpGet]
        public IActionResult EditTypeProgram(int? id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            if (id == null) return RedirectToRoute("MyArea", new { area = "Admin", controller = "Program", action = "Index" });

            TypeProgram typeProgramEdit = db.TypesProgram.Find(id);
            ViewBag.Profiles = new SelectList(db.Profiles.Where(s => s.Id != typeProgramEdit.ProfileId), "Id", "Name");
            ViewBag.ProfileId = db.Profiles.FirstOrDefault(s => s.Id == typeProgramEdit.ProfileId).Id;
            ViewBag.ProfileName = db.Profiles.FirstOrDefault(s => s.Id == typeProgramEdit.ProfileId).Name;
            return View(typeProgramEdit);
        }

        [HttpPost]
        public IActionResult EditTypeProgram(TypeProgram typeProgram)
        {
            TypeProgram typeProgramUpdate = db.TypesProgram.Find(typeProgram.Id);
            typeProgramUpdate.Name = typeProgram.Name;
            typeProgramUpdate.ProfileId = typeProgram.ProfileId;
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Program", action = "Index", id = typeProgram.ProfileId });
        }

        [HttpGet]
        public IActionResult DeleteTypeProgram(int? id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            return View(db.TypesProgram.Find(id));
        }

        [HttpPost]
        public IActionResult DeleteTypeProgram(TypeProgram typeProgram)
        {  
            foreach (Discipline discipline in db.Disciplines.Where(s => s.TypeProgramId == typeProgram.Id))
            {
                var dir = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "files", discipline.Content));
                dir.Delete();
                db.Disciplines.Remove(discipline);
            }
            db.TypesProgram.Remove(typeProgram);
            db.SaveChanges();
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Program", action = "Index", id = typeProgram.ProfileId });
        }

        [HttpGet]
        public IActionResult CreateDiscipline(int? id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            ViewBag.TypesProgram = new SelectList(db.TypesProgram.Where(s => s.ProfileId == id), "Id", "Name");
            ViewBag.ProfileId = id;
            return View();
        }

        private string UploadFile(IFormFileCollection fileColletction)
        {
            var files = fileColletction;
            var formFile = files[0];
            var upFileName = formFile.FileName;

            var fileName = upFileName;
            var saveDir = @".\wwwroot\files\";
            var savePath = saveDir + fileName;
            var previewPath = "/files/" + fileName;
            using (FileStream fs = System.IO.File.Create(savePath))
            {
                formFile.CopyTo(fs);
                fs.Flush();
            }
            return upFileName;
        }

        [HttpPost]
        public IActionResult CreateDiscipline()
        {
            string fileName = UploadFile(Request.Form.Files);

            Discipline disciplineAdd = new Discipline
            {
                Code = Request.Form.FirstOrDefault(p => p.Key == "Code").Value,
                Name = Request.Form.FirstOrDefault(p => p.Key == "Name").Value,
                TypeProgramId = Convert.ToInt32(Request.Form.FirstOrDefault(p => p.Key == "TypeProgramId").Value),
                Content = fileName
            };

            db.Disciplines.Add(disciplineAdd);
            db.SaveChanges();

            TypeProgram typeProgram = db.TypesProgram.FirstOrDefault(s => s.Id == disciplineAdd.TypeProgramId);
            Profile profile = db.Profiles.FirstOrDefault(s => s.Id == typeProgram.ProfileId);
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Program", action = "Index", id = profile.Id });
        }

        [HttpGet]
        public IActionResult EditDiscipline(int? profile_id, int? disc_id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            Discipline disciplineEdit = db.Disciplines.Find(disc_id);
            ViewBag.TypesProgram = new SelectList(db.TypesProgram.Where(s => s.ProfileId == profile_id), "Id", "Name");
            ViewBag.TypeProgramId = db.TypesProgram.FirstOrDefault(s => s.Id == disciplineEdit.TypeProgramId).Id;
            ViewBag.TypeProgramName = db.TypesProgram.FirstOrDefault(s => s.Id == disciplineEdit.TypeProgramId).Name;
            ViewBag.ProfileId = profile_id;
            return View(disciplineEdit);
        }

        [HttpPost]
        public IActionResult EditDiscipline(Discipline discipline, bool? isChooseNewFile)
        {
            Discipline disciplineUpdate = db.Disciplines.Find(discipline.Id);
            disciplineUpdate.Code = Request.Form.FirstOrDefault(p => p.Key == "Code").Value;
            disciplineUpdate.Name = Request.Form.FirstOrDefault(p => p.Key == "Name").Value;
            disciplineUpdate.TypeProgramId = Convert.ToInt32(Request.Form.FirstOrDefault(p => p.Key == "TypeProgramId").Value);

            if (Convert.ToString(isChooseNewFile).Length > 0)
            {
                var fileInfo = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "files", disciplineUpdate.Content));
                fileInfo.Delete();

                disciplineUpdate.Content = UploadFile(Request.Form.Files);
            }

            db.SaveChanges();
            TypeProgram typeProgram = db.TypesProgram.FirstOrDefault(p => p.Id == disciplineUpdate.TypeProgramId);
            Profile profile = db.Profiles.FirstOrDefault(p => p.Id == typeProgram.ProfileId);
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Program", action = "Index", id = profile.Id });
        }

        [HttpGet]
        public IActionResult DeleteDiscipline(int? profile_id, int? disc_id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            Discipline disciplineDelete = db.Disciplines.Find(disc_id);
            ViewBag.TypeProgramId = db.TypesProgram.FirstOrDefault(s => s.Id == disciplineDelete.TypeProgramId).Id;
            ViewBag.ProfileId = profile_id;
            return View(disciplineDelete);
        }

        [HttpPost]
        public IActionResult DeleteDiscipline(Discipline discipline)
        {
            var disciplineContentFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "files", discipline.Content));
            disciplineContentFile.Delete();

            db.Disciplines.Remove(discipline);
            db.SaveChanges();

            TypeProgram typeProgram = db.TypesProgram.FirstOrDefault(p => p.Id == discipline.TypeProgramId);
            Profile profile = db.Profiles.FirstOrDefault(p => p.Id == typeProgram.ProfileId);
            return RedirectToRoute("MyArea", new { area = "Admin", controller = "Program", action = "Index", id = profile.Id });
        }       
    }
}
