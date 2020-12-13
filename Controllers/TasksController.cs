using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Management_Platform.Models;

namespace Task_Management_Platform.Controllers
{
    public class TasksController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();


        //SHOW
        //GET: afisarea unui singur task
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

            Tasks task = db.Tasks.Find(id);
            ViewBag.seteazaStatus = false;
            if (User.IsInRole("Membru") || User.IsInRole("Admin"))
            {
                ViewBag.seteazaStatus = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.esteOrganizator = User.IsInRole("Organizator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            return View(task);
        }

        // pt COMMENTS
        //POST: adaugare cometariu-ul nou in baza de date
        [Authorize(Roles = "Membru,Organizator,Admin")]
        [HttpPost]
        public ActionResult Show(Comment newComment)
        {
            newComment.DataAdaug = DateTime.Now;
            newComment.UserId = User.Identity.GetUserId();

            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(newComment);
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost adaugat cu success!";

                    return Redirect("/Tasks/Show/" + newComment.TaskId);
                }

                Tasks task = db.Tasks.Find(newComment.TaskId);
                return View(task);
            }
            catch (Exception e)
            {
                Tasks task = db.Tasks.Find(newComment.TaskId);
                return View(task);
            }
        }


        //NEW
        //GET: afisare formular adaugare task
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult New()
        {
            return View();
        }

        //POST: adaugare task-ul nou in baza de date
        [Authorize(Roles = "Organizator,Admin")]
        [HttpPost]
        public ActionResult New(Tasks newTask)
        {
            try
            {
                newTask.Status = "Not Started";
                newTask.UserId = User.Identity.GetUserId();
                //if (ModelState.IsValid)
                //{
                    db.Tasks.Add(newTask);
                    db.SaveChanges();
                    TempData["message"] = "Taskul a fost adaugat cu success!";

                    ViewBag.esteAdmin = User.IsInRole("Admin");
                    ViewBag.esteMembru = User.IsInRole("Membru");
                    ViewBag.utilizatorCurent = User.Identity.GetUserId();
                    return Redirect("/Teams/Show/" + newTask.TeamId);
                //}

                ViewBag.Message = "Nu s-a putut adauga task-ul!";
                return View(newTask);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;//"Nu s-a putut adauga task-ul!";
                return View(newTask);
            }
        }

        
        //EDIT
        //GET: afisare formular de editare task
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult Edit(int id)
        {
            Tasks task = db.Tasks.Find(id);

            if (User.Identity.GetUserId() == task.UserId || User.IsInRole("Admin"))
            {
                return View(task);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa modificati un task care nu va apartine!";
                return Redirect("/Teams/Show/" + task.TeamId);
            }
        }

        //PUT: modificare task
        [Authorize(Roles = "Organizator,Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Tasks editedTask)
        {
            try
            {
                if (User.Identity.GetUserId() == editedTask.UserId || User.IsInRole("Admin"))
                {
                    if (ModelState.IsValid)
                    {
                        Tasks task = db.Tasks.Find(id);

                        if (TryUpdateModel(task))
                        {
                            task = editedTask;
                            db.SaveChanges();
                            TempData["message"] = "Task-ul a fost modificat cu succes!";

                            return Redirect("/Tasks/Show/" + id);
                        }

                        ViewBag.Message = "Nu s-a putut edita task-ul!";
                        return View(editedTask);
                    }

                    return View(editedTask);
                }

                else
                {
                    TempData["message"] = "Nu aveti dreptul sa modificati un task care nu va apartine!";
                    return Redirect("/Teams/Show/" + editedTask.TeamId);
                }
            }

            catch (Exception e)
            {
                ViewBag.Message = "Nu s-a putut edita task-ul!";
                return View(editedTask);
            }
        }

        //DELETE
        //DELETE: stergerea unui task
        [Authorize(Roles = "Organizator,Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Tasks task = db.Tasks.Find(id);

            try
            {
                if (User.Identity.GetUserId() == task.UserId || User.IsInRole("Admin"))
                {
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                    TempData["message"] = "Task-ul a fost sters cu success!";

                    return Redirect("/Teams/Show/" + task.TeamId);
                }

                else
                {
                    TempData["message"] = "Nu aveti dreptul sa stergeti un task care nu va apartine!";
                    return Redirect("/Teams/Show/" + task.TeamId);
                }
            }
            catch (Exception e)
            { 
                TempData["message"] = "Nu s-a putut sterge task-ul!";
                return Redirect("/Tasks/Show/" + task.TaskId);
            }
        }
    }
}