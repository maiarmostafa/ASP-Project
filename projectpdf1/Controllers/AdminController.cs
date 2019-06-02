using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectpdf1.Models;

namespace projectpdf1.Controllers
{
    public class AdminController : Controller
    {
        PMrealstateEntities db = new PMrealstateEntities();
        User User;
        // GET: Admin
        public ActionResult Index()
        {
            if ((Convert.ToInt32(Session["UserType"]) == 1))
            {
                var users = db.Users.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
        [HttpGet]
        public ActionResult Adduser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Adduser(User User, HttpPostedFileBase photo)
        {

            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(photo.FileName);
                    string extention = Path.GetExtension(photo.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extention;
                    User.U_Pathphoto = "~/Images/" + filename;
                    filename = Server.MapPath("~/Images/") + filename;
                    photo.SaveAs(filename);

                }
                db.Users.Add(User);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");

            }
            return View();

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["CurrrentUser"] = user;
                return View(user);
            }
        }


        [HttpPost]
        public ActionResult Edit(User user, HttpPostedFileBase photo)
        {

            var U = db.Users.Single(u => u.U_Id == user.U_Id);
            if (ModelState.IsValid)
            {

                if (photo != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(photo.FileName);
                    string extention = Path.GetExtension(photo.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extention;
                    user.U_Pathphoto = "~/Images/" + filename;
                    filename = Server.MapPath("~/Images/") + filename;
                    photo.SaveAs(filename);

                }
                U.U_FName = user.U_FName;
                U.ULName = user.ULName;
                U.U_Phone = user.U_Phone;
                U.U_Job = user.U_Job;
                U.U_Email = user.U_Email;
                U.U_Password = user.U_Password;
                U.Type_id = user.Type_id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Delete(int id)
        {
            var user = db.Users.Single(u => u.U_Id == id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Myprofile()
        {
            if ((Convert.ToInt32(Session["UserType"]) == 1))
            {
                int UserID = Convert.ToInt32(Session["UserID"]);

                User = db.Users.Find(UserID);

                return View(User);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult Addproject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Addproject(Project project, UsersAccepet adccept)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            if (ModelState.IsValid)
            {

                project.User_Id = userid;

                project.Onprog_id = 1;
                project.State_Id = 2;

                db.Projects.Add(project);

                adccept.Project_id = project.Project_Id;
                adccept.User_id = userid;
                adccept.state_id = 2;
                

                db.UsersAccepets.Add(adccept);
                db.SaveChanges();
                return RedirectToAction("ListP", "Admin");
            }
            return View();
        }

        public ActionResult ListP()
        {
            var project = db.Projects.ToList();
            return View(project);
        }
        [HttpGet]
        public ActionResult EditList(int id)
        {



            Project prjectData = db.Projects.Find(id);
            if (prjectData == null)
            {
                return RedirectToAction("ListP");
            }
            else
            {
                ViewData["CurrentProject"] = prjectData;
                return View(prjectData);
            }
            
        }
        [HttpPost]
        public ActionResult EditList( Project projectDatat)
        {
          
            var U = db.Projects.Single(u => u.Project_Id == projectDatat.Project_Id);
            if (ModelState.IsValid)
            {


                U.Post_name = projectDatat.Post_name;
                U.Post_content = projectDatat.Post_content;

                db.SaveChanges();
                return RedirectToAction("ListP");
            }

            return View(projectDatat);
        }
        public ActionResult Deleteproject(int id )
        {
            var proj = db.Projects.Single(x => x.Project_Id == id);
            db.Projects.Remove(proj);
            db.SaveChanges();
            return RedirectToAction("ListP");

        }


        public ActionResult Requests()
        {
            // var requests = db.HiringRequests.ToList();
            var requests = db.HiringRequests.Where(x => x.RequestType == 1);

            return View(requests);
        }


        [HttpGet]
        public ActionResult Approvepost(int id , UsersAccepet AcceptRequest)

        {
            var Request = db.HiringRequests.Single(x => x.R_Id == id);
            var projectState = db.Projects.Single(x => x.Project_Id == Request.Project_id);
            projectState.State_Id = 2;
            AcceptRequest.Project_id = Request.Project_id;
            AcceptRequest.User_id = Request.User_id;
            AcceptRequest.state_id = 2;
            db.HiringRequests.Remove(Request);
            db.UsersAccepets.Add(AcceptRequest);
            db.SaveChanges();

            return RedirectToAction("Requests");
        }
       
        
        public ActionResult Rejectpost(int id)
        {
            var Request = db.HiringRequests.Single(x => x.R_Id == id);
            var DeleteProject = db.Projects.Single(x => x.Project_Id == Request.Project_id);
            db.HiringRequests.Remove(Request);
            db.Projects.Remove(DeleteProject);
            db.SaveChanges();



            return RedirectToAction("Requests");
        }
      
        public ActionResult DetailsRequest(int id )
        {


            var Request = db.HiringRequests.Single(x => x.R_Id == id);

            Project prjectData = db.Projects.Find(Request.Project_id);

            if (prjectData == null)
            {
                return RedirectToAction("Requests");
            }
            else
            {
                ViewData["CurrentProject"] = prjectData;
                return View(prjectData);
            } 
            return View(prjectData);
        }
      
    }
}