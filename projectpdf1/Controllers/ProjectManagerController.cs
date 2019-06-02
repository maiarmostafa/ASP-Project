using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectpdf1.Models;
namespace projectpdf1.Controllers
{
    public class ProjectManagerController : Controller
    {
        PMrealstateEntities db = new PMrealstateEntities();

        // GET: ProjectManager
        public ActionResult Index()
        {
            var project = db.Projects.Where(x => x.State_Id == 2);
            return View(project);
        }

        public ActionResult Request(HiringRequest req , int id)
        {
            req.Project_id = id;
            int userid = Convert.ToInt32(Session["UserID"]);
            req.User_id = userid;
            req.State_id = 2;
            req.RequestType = 1;
            db.HiringRequests.Add(req);
            db.SaveChanges();
            return View("Index");
        }

        public ActionResult ListProjectPM()
        {
            User user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
            var proj = db.Teams.Where(x => x.U_id_PM == user.U_Id);
            
            return View(proj);
        }

        public ActionResult Details(int id)
        {
            var projectDetails = db.Projects.Where(x => x.Project_Id == id);

            return View(projectDetails);
        }

        public ActionResult LeaveProject(int id)
        {
            var Leave = db.Teams.Single(x => x.Team_Id == id);
            Leave.U_id_PM = null;
            db.SaveChanges();

            return RedirectToAction("ListProjectPM");
        }

        public ActionResult RemoveJE(int id)
        {
            var Leave = db.Teams.Single(x => x.Team_Id == id);
            Leave.U_id_JE = null;
            db.SaveChanges();

            return RedirectToAction("ListProjectPM");
        }
        public ActionResult RemoveTL(int id)
        {
            var Leave = db.Teams.Single(x => x.Team_Id == id);
            Leave.U_id_TL = null;
            db.SaveChanges();

            return RedirectToAction("ListProjectPM");
        }



    }
}