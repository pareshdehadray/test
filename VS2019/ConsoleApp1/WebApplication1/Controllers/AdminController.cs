using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private hackathon2019Entities db = new hackathon2019Entities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Location()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            Session["IsLoggedIn"] = "false";
            Session["IsAdmin"] = "false";
            Session["IsVendor"] = "false";

            string password = Utility.MD5Generator.CalculateMD5Hash(loginModel.Password);

            var userData = db.cnf_users.Where(x => x.username == loginModel.UserID && x.password == password);

            if (userData.Count() >= 1)
            {
                Session["UserName"] = loginModel.UserID;

                var firstUserData = userData.First();

                if (firstUserData.password_changed == 0)
                {
                    return RedirectToAction("ChangePassword");
                }

                Session["IsLoggedIn"] = "true";

                FormsAuthentication.SetAuthCookie(loginModel.UserID, true);

                if (firstUserData.role.ToLower() == "admin")
                {
                    HttpContext.User = new GenericPrincipal(new GenericIdentity(loginModel.UserID, "forms"), new string[] { "Admin" });
                    Session["IsAdmin"] = "true";
                    return RedirectToAction("Index", "Admin");
                }
                else if (firstUserData.role.ToLower() == "vendor")
                {
                    HttpContext.User = new GenericPrincipal(new GenericIdentity(loginModel.UserID, "forms"), new string[] { "Vendor" });
                    Session["IsVendor"] = "true";
                    return RedirectToAction("Index", "Vendor");
                }
                else
                {
                    HttpContext.User = new GenericPrincipal(new GenericIdentity(loginModel.UserID, "forms"), new string[] { "Normal" });
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid User ID or Password");
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserID = Session["UserName"].ToString();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Password) || string.IsNullOrEmpty(loginModel.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password and confirm password should not be blank");
            }

            if (!string.IsNullOrEmpty( loginModel.Password) && !string.IsNullOrEmpty(loginModel.ConfirmPassword) && loginModel.Password != loginModel.ConfirmPassword)
            {
                ModelState.AddModelError("", "Password and confirm password should be same.");
            }

            if (ModelState.IsValid)
            {
                string password = Utility.MD5Generator.CalculateMD5Hash(loginModel.OldPassword);

                string userid = Session["UserName"].ToString();

                var userData = db.cnf_users.Where(x => x.username == userid && x.password == password);

                if (userData.Count() == 0)
                {
                    ModelState.AddModelError("", "Old password is not correct");
                }
                else
                {
                    var user = userData.First();
                    string newPassword = Utility.MD5Generator.CalculateMD5Hash(loginModel.ConfirmPassword);
                    user.password = newPassword;
                    user.password_changed = 1;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

    }
}