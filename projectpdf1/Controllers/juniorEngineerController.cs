using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectpdf1.Models;
namespace projectpdf1.Controllers
{
    public class juniorEngineerController : Controller
    {
        PMrealstateEntities db = new PMrealstateEntities();
        // GET: juniorEngineer
        public ActionResult Index()
        { 

            var requests = db.HiringRequests.Where(x => x.RequestType == 4);

            return View(requests);
        }

        public ActionResult Accept(int id , HiringRequest req )
        {
            var proj = db.HiringRequests.Single(F => F.R_Id == id);

            var t = db.Teams.Where(S => S.project_id == proj.Project_id).FirstOrDefault();

            t.U_id_JE = Convert.ToInt32(Session["UserID"]);
            db.HiringRequests.Remove(proj);

            db.SaveChanges();


            return RedirectToAction("Index");
        }
        public ActionResult Refuse(int id )
        {
            var proj = db.HiringRequests.Single(x => x.R_Id == id);
            db.HiringRequests.Remove(proj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ListProjectJE()
        {
            User user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
             var proj = db.Teams.Where(x => x.U_id_JE == user.U_Id);
            
            return View(proj);
        }
        public ActionResult Details(int id)
        {
            var projectDetails = db.Projects.Where(x => x.Project_Id == id);

            return View(projectDetails);
        }
        public ActionResult JEProfile()
        {
            int UserID = Convert.ToInt32(Session["UserID"]);

           User User = db.Users.Find(UserID);

            return View(User);
        }

        public ActionResult LeaveProject(int id)
        {
            
            var Leave = db.Teams.Single(x => x.project_id == id);
            Leave.U_id_JE = null;
            db.SaveChanges();

            return RedirectToAction("ListProjectJE");
        }
    }
}