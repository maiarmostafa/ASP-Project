using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectpdf1.Models;

namespace projectpdf1.Controllers
{
    public class HomeController : Controller
    {
        PMrealstateEntities db= new PMrealstateEntities();
        // GET: Home
        
        public ActionResult Index()
        {
            return View("index");
        }

        public ActionResult Login()
        {
            return PartialView("_Login");
        }
        [HttpPost]
        public ActionResult Login(User User)
        {
            db = new PMrealstateEntities();
            var userVaild = db.Users.Where(x => x.U_Email == User.U_Email && x.U_Password == User.U_Password).FirstOrDefault();
            if (userVaild != null)
            {

                Session["UserID"] = userVaild.U_Id;
                Session["UserFirstName"] = userVaild.U_FName.ToString();
                Session["UserLastName"] = userVaild.ULName.ToString();
                Session["UserType"] = userVaild.Type_id;
                if (Session["UserType"].Equals(1))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (Session["UserType"].Equals(2))
                {
                    return RedirectToAction("Index", "PM");
                }
                else if (Session["UserType"].Equals(3))
                {
                    return RedirectToAction("Index", "TeamLeader");
                }
                else if (Session["UserType"].Equals(4))
                {
                    return RedirectToAction("Index", "juniorEngineer");
                }
                else if (Session["UserType"].Equals(5))
                {
                    return RedirectToAction("myProfile", "Customer");
                }
                else
                {
                    ViewBag.Login = "Email Or Password Not Vaild .";
                    return RedirectToAction("Login");
                }

            }
            else
            {
                ViewBag.Login = "Email Or Password Not Vaild .";
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return View("Index");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return PartialView("_Register");
        }
        [HttpPost]
        public ActionResult Register(User User,HttpPostedFileBase photo)
        {
            db = new PMrealstateEntities();
          
            if (photo != null)
            {
                string filename = Path.GetFileNameWithoutExtension(photo.FileName);
                string extention = Path.GetExtension(photo.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extention;
                User.U_Pathphoto = "~/Images/" + filename;
                filename = Server.MapPath("~/Images/") + filename;
                photo.SaveAs(filename);

            }
            if (ModelState.IsValid)
            {
                if (!db.Users.Any(x => x.U_Email == User.U_Email))
                {
                    db.Users.Add(User);
                    db.SaveChanges();
                    var userVaild = db.Users.Where(x => x.U_Email == User.U_Email && x.U_Password == User.U_Password).FirstOrDefault();
                    if (userVaild != null)
                    {
                        Session["UserID"] = userVaild.U_Id;
                        Session["UserFirstName"] = userVaild.U_FName.ToString();
                        Session["UserLastName"] = userVaild.ULName.ToString();
                        Session["UserType"] = userVaild.Type_id;

                        if (Session["UserType"].Equals(1))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (Session["UserType"].Equals(2))
                        {
                            return RedirectToAction("Index", "PM");
                        }
                        else if (Session["UserType"].Equals(3))
                        {
                            return RedirectToAction("Index", "TeamLeader");
                        }
                        else if (Session["UserType"].Equals(4))
                        {
                            return RedirectToAction("Index", "juniorEngineer");
                        }
                        else if (Session["UserType"].Equals(5))
                        {
                            return RedirectToAction("myProfile", "Customer");
                        }

                    }
                }
                ViewBag.Message = "User With This Email Already Exist";

                
            }
            return View("Index");
         
        }
        
    }
}