using Task_Management_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Task_Management_Platform.Controllers
{
    public class TeamsController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        // GET: Teams
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];
            var teams = db.Teams;
            ViewBag.Teams = teams;
            return View();
        }
        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

           Team team = db.Teams.Find(id);

            return View(team);
        }
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(Team newTeam)
        {
            newTeam.DataInscriere = DateTime.Now;
            try
            {
                db.Teams.Add(newTeam);
                db.SaveChanges();
                TempData["message"]= "Echipa a fost adaugata!";
                return RedirectToAction("Index");
            }catch(Exception e)
            {
                ViewBag.Message= "Nu s-a putut adauga echipa.";
                return View(newTeam);
            }
       
        }

        //Afisarea formularului de editare
        public ActionResult Edit(int id)
        {
            Team team;
            team=db.Teams.Find(id);
            return View(team);
        }

        //Editarea propriu-zisa din care preluam informatiile
        //din formular in baza de date
        [HttpPut]
        public ActionResult Edit(int id,Team teamReq)
        {
            try
            {
                Team team = db.Teams.Find(id);
                if (TryUpdateModel(team))
                {
                    team = teamReq;
                    db.SaveChanges();
                    TempData["message"] = "Echipa a fost editata!";
                    return Redirect("/Teams/Show/"+id);
                }

            }catch(Exception e)
            {
                ViewBag.message = "Echipa nu exista sau nu se poate edita!";
                return View(teamReq);
            }
            ViewBag.message = "Echipa nu exista sau nu se poate edita!";
            return View(teamReq);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                Team team = db.Teams.Find(id);
                if(TryUpdateModel(team))
                {
                    db.Teams.Remove(team);
                    db.SaveChanges();
                    TempData["message"] = "Echipa a fost stearsa cu succes!";
                    return RedirectToAction("Index");
                }

            }catch(Exception e)
            {
                ViewBag.message = "Echipa nu a fost gasita sau nu poate fi stearsa!";
                return View("/Teams/Show/"+id);
            }
            ViewBag.message = "Echipa nu a fost gasita sau nu poate fi stearsa!";
            return View("/Teams/Show/" + id);
        }
    }
}