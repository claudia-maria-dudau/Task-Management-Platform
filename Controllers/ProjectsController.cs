using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Management_Platform.Models;


namespace Task_Management_Platform.Controllers
{
    public class ProjectsController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        public ActionResult New(int Id)
        {
            ViewBag.TeamId = Id;
            return View();
        }

        [HttpPost]
        public ActionResult New(Project project)
        {
            try
            {
                db.Projects.Add(project);
                TempData["message"] = "Proiectul a fost adaugat cu succes.";
                db.SaveChanges();
                return Redirect("/Teams/Show/" + project.TeamId);
            }catch(Exception e)
            {
                ViewBag.Message = "Nu s-a putut adauga proiectul.";
                return View(project);
            }
        }

        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];
            Project project = db.Projects.Find(id);
            return View(project);
        }

        public ActionResult Edit(int id)
        {
            Project proj = db.Projects.Find(id);
            return View(proj);
        }
        [HttpPut]
        public ActionResult Edit(int id,Project projectNew)
        {
            try
            {
                Project project = db.Projects.Find(id);
                if (TryUpdateModel(project))
                {
                    project = projectNew;
                    db.SaveChanges();
                    TempData["message"] = "Proiectul a fost editat cu succes!";
                    return Redirect("/Projects/Show/" + id);
                }

            }
            catch(Exception e)
            {
                ViewBag.Message = "Nu s-a putut adauga proiectul.";
                return View(projectNew);
            }
            return View(projectNew);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                Project prj = db.Projects.Find(id);
                if (TryUpdateModel(prj))
                {
                    var team_ID = prj.TeamId;
                    db.Projects.Remove(prj);
                    db.SaveChanges();
                    TempData["message"] = "Proiectul a fost sters cu succes!";
                    return Redirect("/Teams/Show/"+team_ID);
                }

            }
            catch (Exception e)
            {
                ViewBag.message = "Proiectul nu poate fi sters!";
                return View("/Projects/Show/" + id);
            }
            ViewBag.message = "Proiectul nu poate fi sters";
            return View("/Projects/Show/" + id);
        }
    }
}