using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Management_Platform.Models;

namespace Task_Management_Platform.Controllers
{
    public class CommentsController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        //EDIT
        //GET: afisare formular de editare comentariu
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);

            if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(comment);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Tasks");
            }
        }

        //PUT: modificare comentariu
        [HttpPut]
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Edit(int id, Comment editedComment)
        {
            try
            {
                Comment comment = db.Comments.Find(id);
                if (ModelState.IsValid)
                {
                    if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(comment))
                        {
                            comment.Content = editedComment.Content;
                            db.SaveChanges();
                        }
                        TempData["message"] = "Comentariul a fost editat cu succes!";
                        return Redirect("/Tasks/Show/" + comment.TaskId);
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                        return Redirect("/Tasks/Show/" + comment.TaskId);
                    }
                }
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return Redirect("/Tasks/Show/" + comment.TaskId);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Comentariul nu a putut fi editat!";
                return View(editedComment);
            }
        }

        //DELETE
        //DELETE: stergerea unui comentariu
        [HttpDelete]
        [Authorize(Roles = "Membru,Organizator,Admin")]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);

            try
            {
                if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {

                    db.Comments.Remove(comment);
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost sters cu success!";

                    return Redirect("/Tasks/Show/" + comment.TaskId);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                    return RedirectToAction("Index", "Tasks");
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                TempData["message"] = "A aparut o eroare la stergerea mesajului!";
                return Redirect("/Tasks/Show/" + comment.Task.TaskId);
            }
        }
    }
}