using Task_Management_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Task_Management_Platform.Controllers
{
    public class TeamsController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        [Authorize(Roles = "User,Member,Organizator,Admin")]
        // GET: Teams
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

            var teams = db.Teams;
            ViewBag.Teams = teams;
            return View();
        }

        [Authorize(Roles = "Member,Organizator,Admin")]
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

            Team team = db.Teams.Find(id);
            bool apartine = false;
            foreach (ApplicationUser member in team.Members)
            {
                if (member.Id == User.Identity.GetUserId())
                {
                    apartine = true;
                }
            }

            if (apartine || User.IsInRole("Admin"))
            {
                ViewBag.esteMembru = User.IsInRole("Membru");
                ViewBag.esteOrganizator = User.IsInRole("Organizator");
                ViewBag.esteAdmin = User.IsInRole("Admin");
                ViewBag.utilizatorCurent = User.Identity.GetUserId();
                return View(team);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul de a viziona detaliile despre o echipa din care nu faceti parte!";
                return RedirectToAction("Index");
            }
        }


        [Authorize(Roles = "User,Member,Organizator,Admin")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "Organizator,Admin")]
        [HttpPost]
        public ActionResult New(Team newTeam)
        {
            newTeam.DataInscriere = DateTime.Now;
            string userId = User.Identity.GetUserId();
            newTeam.UserId = userId;

            try
            {
                //modificare rol utilizator in Organizator
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                UserManager.RemoveFromRole(userId, "User");
                UserManager.RemoveFromRole(userId, "Membru");

                UserManager.AddToRole(userId, "Organizator");


                db.Teams.Add(newTeam);
                db.SaveChanges();
                TempData["message"]= "Echipa a fost adaugata!";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.Message= "Nu s-a putut adauga echipa.";
                return View(newTeam);
            }
       
        }

        //Afisarea formularului de editare
        [Authorize(Roles = "Organizator,Admin")]
        public ActionResult Edit(int id)
        {
            Team team;
            team = db.Teams.Find(id);

            if (team.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(team);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul de a edita o echipa din care nu faceti parte!";
                return RedirectToAction("Index");
            }
        }

        //Editarea propriu-zisa din care preluam informatiile
        //din formular in baza de date
        [Authorize(Roles = "Organizator,Admin")]
        [HttpPut]
        public ActionResult Edit(int id,Team teamReq)
        {
            try
            {
                if (teamReq.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    Team team = db.Teams.Find(id);
                    if (TryUpdateModel(team))
                    {
                        team = teamReq;
                        db.SaveChanges();
                        TempData["message"] = "Echipa a fost editata!";
                        return Redirect("/Teams/Show/" + id);
                    }

                    else
                    {
                        TempData["message"] = "Nu aveti dreptul de a edita o echipa din care nu faceti parte!";
                        return RedirectToAction("Index");
                    }
                }

            }catch(Exception e)
            {
                ViewBag.message = "Echipa nu exista sau nu se poate edita!";
                return View(teamReq);
            }
            ViewBag.message = "Echipa nu exista sau nu se poate edita!";
            return View(teamReq);
        }

        [Authorize(Roles = "Organizator,Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                Team team = db.Teams.Find(id);
                if (team.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(team))
                    {
                        db.Teams.Remove(team);
                        db.SaveChanges();
                        TempData["message"] = "Echipa a fost stearsa cu succes!";
                        return RedirectToAction("Index");
                    }

                    TempData["message"] = "Echipa nu a fost gasita sau nu poate fi stearsa!";
                    return View("/Teams/Show/" + id);
                }

                else
                {
                    TempData["message"] = "Nu aveti dreptul de a sterge o echipa din care nu faceti parte!";
                    return RedirectToAction("Index");
                }

            }
            catch(Exception e)
            {
                TempData["message"] = "Echipa nu a fost gasita sau nu poate fi stearsa!";
                return View("/Teams/Show/"+id);
            }
        }
    }
}