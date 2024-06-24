using MVCproject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCproject.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext db = new AppDbContext();

        #region user registration 

        public ActionResult Register(string returnUrl)
        {
            TempData.Keep("RedirectTo");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Register(User t)
        {
            if (ModelState.IsValid)
            {
                User u = new User
                {
                    Name = t.Name,
                    Email = t.Email,
                    Password = t.Password,
                    RoleType = 2
                };
                db.Users.Add(u);
                db.SaveChanges();

                // Automatically log in the user
                Session["uid"] = u.UserId;
                FormsAuthentication.SetAuthCookie(u.Email, false);
                Session["User"] = u.Name;

                // If RedirectTo is set, redirect to it
                if (TempData["RedirectTo"] != null && Url.IsLocalUrl(TempData["RedirectTo"].ToString()))
                {
                    string redirectTo = TempData["RedirectTo"].ToString();
                    TempData.Keep("RedirectTo"); // Ensure TempData is kept for one more request
                    return Redirect(redirectTo);
                }

                // Redirect to the appropriate home based on role
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Not Register!!";
            }
            return View();
        }



        #endregion



        #region user login

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            TempData.Keep("RedirectTo");
            ViewBag.ReturnUrl = returnUrl ?? TempData["RedirectTo"] as string;
            return View();
        }

        [HttpPost]
        public ActionResult Login(User t, string returnUrl)
        {
            var query = db.Users.SingleOrDefault(m => m.Email == t.Email && m.Password == t.Password);
            if (query != null)
            {
                Session["uid"] = query.UserId;
                FormsAuthentication.SetAuthCookie(query.Email, false);
                Session["User"] = query.Name;

                // If returnUrl is valid, redirect to it
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                // If returnUrl is not set, check TempData for RedirectTo value
                if (TempData["RedirectTo"] != null && Url.IsLocalUrl(TempData["RedirectTo"].ToString()))
                {
                    return Redirect(TempData["RedirectTo"].ToString());
                }

                // Role-based redirection if no returnUrl is provided
                if (query.RoleType == 1)
                {
                    return RedirectToAction("Index", "Home"); // Admin Home
                }
                else if (query.RoleType == 2)
                {
                    return RedirectToAction("Index", "Home"); // Customer Home
                }
            }
            else
            {
                TempData["msg"] = "Invalid Username or Password";
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }




        #endregion


        #region logout 

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            TempData.Remove("cart");
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}