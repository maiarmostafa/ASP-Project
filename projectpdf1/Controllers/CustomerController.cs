using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using projectpdf1.Models;

namespace projectpdf1.Controllers
{/*
    on_prog 
            1 = on progress
            2= deliver

    state 
        1=pinding 
        2=accept
   request state 
           1 = customerToAdmin
           2 = PM
     */


    public class CustomerController : Controller
    {
      //  HomeController obj = new HomeController();
        

        PMrealstateEntities db = new PMrealstateEntities();

        // GET: Customer
        public ActionResult myProfile( )
        {
            /* int userid = Convert.ToInt32(Session["UserID"]);

             User user = db.Users.Find(userid);
             ViewData["CurrrentUser"] = user;
             return View();*/

            if ((Convert.ToInt32(Session["UserType"]) == 5))
            {
                int UserID = Convert.ToInt32(Session["UserID"]);

             User  user = db.Users.Find(UserID);

                return View(user);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        [HttpGet]       
        public ActionResult CustomerPost()
        {


            return View();
        }

        [HttpPost]
        public ActionResult CustomerPost(Project proj, HiringRequest req)
        {

            int userid = Convert.ToInt32(Session["UserID"]);
            proj.User_Id = userid;

            proj.Onprog_id = 1;
            proj.State_Id = 1;
            db.Projects.Add(proj);
            //
            req.Project_id = proj.Project_Id;
            req.User_id = userid;
            req.State_id = proj.State_Id;
            req.RequestType = 1;
            db.HiringRequests.Add(req);
            db.SaveChanges();


            return RedirectToAction("myProfile");
        }
        public ActionResult ListTest()
        {
            var project = db.Projects.Where(x => x.State_Id == 2);
            return View( project);
        }

        public User get(int id)
        {
            var userData = db.Users.Where(x => x.U_Id == id).FirstOrDefault();



            return userData;


        }

         public Project getProjectData(int id)
            {
                var ProjectData = db.Projects.Where(x => x.Project_Id == id).FirstOrDefault();



                return ProjectData;
            }
       
         [HttpGet]
         public ActionResult Edit(int id)
         {
             Project prjectData = db.Projects.Find(id);
             if (prjectData == null)
             {
                 return RedirectToAction("CustomerPost");
             }
             else
             {
                 ViewData["CurrentProject"] = prjectData;

                return View(prjectData);
             }
         }


        [HttpPost]
        public ActionResult Edit(Project projectDatat)
        {

            var U = db.Projects.Single(u => u.Project_Id == projectDatat.Project_Id);
            if (ModelState.IsValid)
            {

                
                U.Post_name = projectDatat.Post_name;
                U.Post_content = projectDatat.Post_content;
               
                db.SaveChanges();
                return RedirectToAction("myProfile");
            }

            return View(projectDatat);
        }


        public ActionResult Delete(int id)
        {
            var HiringDelete = db.HiringRequests.Single(x => x.Project_id == id);
            var proj = db.Projects.Single(x => x.Project_Id == id);
            db.HiringRequests.Remove(HiringDelete);

            db.Projects.Remove(proj);
            db.SaveChanges();

            return RedirectToAction("myProfile");/*
            if (projRequest == null && projTeam == null && projHiring == null  )
            {
                
            }
            else
            {
                return View("CustomerPost");
            }          */
        }

        public ActionResult Assign()
        {
            return View();
        }


        public ActionResult ProjectDeliverd()
        {
           // var Delver = db.Projects.ToList();
            return PartialView("ProjectDeliverd");
        }
    }

}
 