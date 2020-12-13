using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult New(int Id)
        {
            ViewBag.TeamId = Id;
            return View();
        }

        [Authorize(Roles = "Membru,Organizator,Admin")]
        [HttpPost]
        public ActionResult New(Project project)
        {
            string userId = User.Identity.GetUserId();
            project.UserId = userId;

            try
            {
                //modificare rol utilizator in Organizator
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                UserManager.RemoveFromRole(userId, "User");
                UserManager.RemoveFromRole(userId, "Membru");

                UserManager.AddToRole(userId, "Organizator");

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

        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

            Project project = db.Projects.Find(id);
            return View(project);
        }

        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult Edit(int id)
        {
            Project proj = db.Projects.Find(id);

            if (proj.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(proj);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul de a edita un proiect care nu va apartine!";
                return RedirectToAction("Index");
            }

        }

        [Authorize(Roles = "Organizator,Admin")]
        [HttpPut]
        public ActionResult Edit(int id,Project projectNew)
        {
            try
            {
                Project project = db.Projects.Find(id);
                if (project.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(project))
                    {
                        project = projectNew;
                        db.SaveChanges();
                        TempData["message"] = "Proiectul a fost editat cu succes!";
                        return Redirect("/Projects/Show/" + id);
                    }
                }

                else
                {
                    TempData["message"] = "Nu aveti dreptul de a edita un proiect care nu va apartine!";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                ViewBag.Message = "Nu s-a putut adauga proiectul.";
                return View(projectNew);
            }
            return View(projectNew);
        }

        [Authorize(Roles = "Organizator,Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                Project prj = db.Projects.Find(id);

                if (prj.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(prj))
                    {
                        var team_ID = prj.TeamId;
                        db.Projects.Remove(prj);
                        db.SaveChanges();
                        TempData["message"] = "Proiectul a fost sters cu succes!";
                        return Redirect("/Teams/Show/" + team_ID);
                    }
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul de a sterge un proiect care nu va apartine!";
                    return RedirectToAction("Index");
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