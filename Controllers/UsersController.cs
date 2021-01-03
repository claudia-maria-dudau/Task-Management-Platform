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
    public class UsersController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        private int perPage = 20;

        // GET: Users
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

            var users = db.Users.OrderBy(u => u.UserName);
            var search = "";

            //cautare
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> userIds = db.Users.Where(u => u.UserName.Contains(search)).Select(u => u.Id).ToList();
                users = db.Users.Where(u => userIds.Contains(u.Id)).OrderBy(u => u.UserName);
            }

            //paginare
            var totalUsers = users.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));
            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * perPage;
            }

            var paginatedUsers = users.Skip(offset).Take(perPage);

            ViewBag.perPage = perPage;
            ViewBag.total = totalUsers;
            ViewBag.lastPage = Math.Ceiling((float)totalUsers / (float)perPage);
            ViewBag.users = paginatedUsers;
            return View();
        }

        //GET: user
        [Authorize(Roles = "Admin")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            //obtinem rolul curent
            var roleName = db.Roles.Find(user.Roles.FirstOrDefault().RoleId).Name;
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                    //user.Nume = editedUser.Nume;
                    //user.UserName = editedUser.UserName;
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

                    TempData["message"] = "Utilizatorul a fost editat!";
                    return RedirectToAction("Index");
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin,Organizator,Membru")]
        public ActionResult AfisareMembri(int id)
        {
            Team team = db.Teams.Find(id);
            var users = team.Users.OrderBy(u => u.UserName);
            var tasks = db.Teams.Find(id).Tasks;

            ViewBag.users = users;
            ViewBag.tasks = tasks;
            return View();
        }
    }
}