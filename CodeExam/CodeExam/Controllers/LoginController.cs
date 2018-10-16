using CodeExam.Models;
using CodeExam.ViewModels;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Controllers
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
                    return RedirectToLocal(ReturnUrl);
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
        public ActionResult Login(AccountViewModel user, string url)
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
                            Session["Role"] = user.RoleID.ToString();
                            Session["ID"] = obj.UserId.ToString();
                            SignInUser(obj.UserName, user.Password, obj.RoleId, user.IsPersistent);
                            return RedirectToLocal(url);
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

        private ActionResult RedirectToLocal(string url)
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
            return RedirectToAction("Index", "Home");
        }

        private void SignInUser(string username, string password, int roleID, bool isPersistene)
        {
            var claims = new List<Claim>();
            try
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim(ClaimTypes.Role, roleID.ToString()));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                if (isPersistene)
                {
                    HttpCookie httpCookieUs = new HttpCookie("username", username);
                    HttpCookie httpCooKiePw = new HttpCookie("password", password);
                    HttpCookie httpCookieRm = new HttpCookie("remember", "1");

                    httpCookieUs.Expires.AddDays(2);
                    httpCooKiePw.Expires.AddDays(2);
                    httpCookieRm.Expires.AddDays(2);

                    Response.Cookies.Add(httpCookieUs);
                    Response.Cookies.Add(httpCooKiePw);
                    Response.Cookies.Add(httpCookieRm);
                }
                else
                {
                    if (Response.Cookies["username"].Value != null || Response.Cookies["password"] != null)
                    {
                        Response.Cookies.Remove("username");
                        Response.Cookies.Remove("password");
                        Response.Cookies.Remove("remember");
                    }
                }
                authenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = isPersistene }, claimIdenties);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize]
        public ActionResult Logout()
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
            return RedirectToAction("Index", "Login");
        }
    }
}