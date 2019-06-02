using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectpdf1.Models;
namespace projectpdf1.Controllers
{
    public class TeamLeaderController : Controller
    {
        PMrealstateEntities db = new PMrealstateEntities();
        // GET: TeamLeader
        public ActionResult Index()
        {
            var requests = db.HiringRequests.Where(x => x.RequestType == 3);

            return View(requests);
        }

        public ActionResult TLProfile()
        {
            int UserID = Convert.ToInt32(Session["UserID"]);

            User User = db.Users.Find(UserID);

            return View(User);
        }

        public ActionResult ListProjectTL()
        {
            User user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
            var proj = db.Teams.Where(x => x.U_id_TL == user.U_Id);

            return View(proj);
        }


        public ActionResult Accept(int id, HiringRequest req)
        { 
            var proj = db.HiringRequests.Single(F => F.R_Id == id);

            var t = db.Teams.Where(S => S.project_id == proj.Project_id).FirstOrDefault();

            t.U_id_TL = Convert.ToInt32(Session["UserID"]);
            db.HiringRequests.Remove(proj);

            db.SaveChanges();


            return RedirectToAction("Index");
        }


        public ActionResult Refuse(int id)
        {
            var proj = db.HiringRequests.Single(x => x.R_Id == id);
            db.HiringRequests.Remove(proj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var projectDetails = db.Projects.Where(x => x.Project_Id == id);

            return View(projectDetails);
        }

        public ActionResult LeaveProject(int id)
        {
            var Leave = db.Teams.Single(x => x.Team_Id == id);
            Leave.U_id_TL = null;
            db.SaveChanges();

            return RedirectToAction("ListProjectTL");
        }


        public ActionResult RemoveJE(int id)
        {
            var Leave = db.Teams.Single(x => x.Team_Id == id);
            Leave.U_id_JE = null;
            db.SaveChanges();

            return RedirectToAction("ListProjectTL");
        }
        public ActionResult AssignJE(int id )
        {
            Session["IDProject"] = id.ToString();
;            return RedirectToAction("ShowAllJE");
        }
        public ActionResult ShowAllJE()
        {   
            var users = db.Users.Where(x=>x.Type_id== 4 );
            return View(users);
        }
        public ActionResult Assign(int id , HiringRequest req)
        {
            
            var test = Convert.ToInt32(Session["IDProject"]);

            req.Project_id = test;
            req.User_id = id;
            req.RequestType = 4;
            req.State_id = 2;
            db.HiringRequests.Add(req);
            db.SaveChanges();
          
            return RedirectToAction("AssignJE");
        }

        [HttpGet]
        public ActionResult Feedback(int id)
        {
            Session["feedback"] = id;

            return View("Feedback");
        }


        [HttpPost]
        public ActionResult Feedback(feedback_ feed , int id )
        {
            var teamData = db.Teams.Single(x => x.Team_Id == id);
            int userid = Convert.ToInt32(Session["UserID"]);
            feed.FromTL = userid;
            feed.TeamID = id;
            feed.ToPM = teamData.U_id_PM;
            feed.OnJE = teamData.U_id_JE;
            db.feedback_.Add(feed);
            db.SaveChanges();
            return View("ListProjectTL");
        }
    }
}