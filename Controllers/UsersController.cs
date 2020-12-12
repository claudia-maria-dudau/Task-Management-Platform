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
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

            var users = db.Users.OrderBy(u => u.UserName);

            ViewBag.Users = users;
            return View();
        }

        //GET: user
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            //obtinem rolul curent
            var roleName = db.Roles.Find(user.Roles.FirstOrDefault().RoleId);
            ViewBag.roleName = roleName;

            return View(user);
        }

        //obtinere a tutror rolurilor din baza de date
        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();
            var roles = db.Roles;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()

                });
            }
            return selectList;
        }

        //GET: formular editare
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            //toate rolurile
            user.AllRoles = GetAllRoles();

            //rolul curent
            ViewBag.userRole = user.Roles.FirstOrDefault().RoleId;

            return View(user);
        }

        //PUT: editare user
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser editedUser)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            ViewBag.userRole = user.Roles.FirstOrDefault().RoleId;

            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


                if (TryUpdateModel(user))
                {
                    user.Nume = editedUser.Nume;
                    user.UserName = editedUser.UserName;
                    user.Email = editedUser.Email;
                    user.PhoneNumber = editedUser.PhoneNumber;

                    //sterge roluri existente alea userului
                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Name);
                    }

                    //adaugare rol nou
                    var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                    UserManager.AddToRole(id, selectedRole.Name);

                    db.SaveChanges();
                }

                TempData["message"] = "Nu s-a putut modifica utilizatorul!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.message = "Nu s-a putut modifica utilizatorul!";
                editedUser.Id = id;
                return View(editedUser);
            }
        }

        //DELETE: stergere user
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var user = UserManager.Users.FirstOrDefault(u => u.Id == id);

                //stergere echipe user
                var teams = db.Teams.Where(tm => tm.UserId == id);
                foreach (var team in teams)
                {
                    db.Teams.Remove(team);
                }

                //stergere task-uri user
                var tasks = db.Tasks.Where(tsk => tsk.UserId == id);
                foreach (var task in tasks)
                {
                    db.Tasks.Remove(task);
                }

                //stergere proiecte user
                var projects = db.Projects.Where(prj => prj.UserId == id);
                foreach (var project in projects)
                {
                    db.Projects.Remove(project);
                }

                //stergere comentarii user
                var comments = db.Comments.Where(comm => comm.UserId == id);
                foreach (var comment in comments)
                {
                    db.Comments.Remove(comment);
                }

                db.SaveChanges();

                UserManager.Delete(user);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["message"] = "Nu s-a putut sterge utilizatorul!";
                return RedirectToAction("Index");
            }
        }
    }
}