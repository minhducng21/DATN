using ASPSnippets.GoogleAPI;
using CodeExam.Models;
using CodeExam.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CodeExam.Areas.Controllers
{
    public class LoginController : Controller
    {
        CodeWarDbContext db = new CodeWarDbContext();

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(AccountViewModel user, string ReturnUrl)
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    return RedirectToLocal(ReturnUrl, null);
                }
                user.IsPersistent = false;
                if (Request.Cookies["username"].Value != "" && Request.Cookies["password"].Value != "")
                {
                    user.UserName = Request.Cookies["username"].Value;
                    user.Password = Request.Cookies["password"].Value;
                    if (Request.Cookies["remember"].Value == "1")
                    {
                        user.IsPersistent = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountViewModel user, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string password = user.Password != null ? Encryption.Encrypt(user.Password) : "";
                    var obj = db.Users.Where(u => user.UserName == u.UserName && password == u.Password).FirstOrDefault();
                    if (obj != null)
                    {
                        try
                        {
                            Session["Role"] = user.RoleId.ToString();
                            Session["ID"] = obj.UserId.ToString();
                            SignInUser(obj.UserName, user.Password, obj.SocialId, obj.RoleId, user.IsPersistent);
                            return RedirectToLocal(ReturnUrl, obj.RoleId);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    else
                        ViewData["Notification"] = "Đăng nhập sai tên tài khoản hoặc mật khẩu";
                }
                catch (Exception ex)
                {
                    Logger logger = LogManager.GetLogger("fileLogger");
                    logger.Error(ex, "Error Login");
                    throw;
                }
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void LoginWithGoogle()
        {
            GoogleConnect.ClientId = "497757490775-h5dkgln71ra20p87aav3e6dqu76jo5es.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "i1NPNkoQhusebPfSJV8JkfVF";
            GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];
            GoogleConnect.Authorize("email");
        }

        [ActionName("LoginWithGoogle")]
        public ActionResult LoginWithGoogleConfirm()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];
                string json = GoogleConnect.Fetch("me", code);
                GoogleAccViewModel profile = new JavaScriptSerializer().Deserialize<GoogleAccViewModel>(json);
                var obj = db.Users.Where(u => u.SocialId == profile.Id).FirstOrDefault();
                if (obj != null)
                {
                    SignInUser(obj.UserName, obj.Password, obj.SocialId, obj.RoleId, false);
                    return RedirectToAction("Index", "Code");
                }
                User user = new User();
                user.DisplayName = profile.DisplayName;
                user.Email = profile.Emails.Find(e => e.Type == "account").Value;
                user.SocialId = profile.Id;
                user.RoleId = (int)RoleCommon.User;
                db.Users.Add(user);
                db.SaveChanges();
                var currentUser = db.Users.Where(u => u.SocialId == profile.Id).FirstOrDefault();
                //Session["ID"] = currentUser.UserId.ToString();
                //Session["Role"] = currentUser.RoleId.ToString();
                SignInUser(currentUser.UserName, currentUser.Password, currentUser.SocialId, currentUser.RoleId, false);
                return RedirectToAction("Index", "Code");
            }
            return RedirectToAction("Index", "Login");
        }

        private ActionResult RedirectToLocal(string url, int? roleId)
        {
            try
            {
                if (Url.IsLocalUrl(url))
                {
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            if (roleId == (int)RoleCommon.User)
            {
                return RedirectToAction("Index", "Code");
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        private void SignInUser(string username, string password, string socialId, int roleId, bool isPersistene)
        {
            var claims = new List<Claim>();
            try
            {
                if (String.IsNullOrEmpty(username))
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleId.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, socialId));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Name, username));
                    claims.Add(new Claim(ClaimTypes.Role, roleId.ToString()));
                }

                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistene }, claimIdenties);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonResult LogOut()
        {
            try
            {
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                //Sign out
                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}