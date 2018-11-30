using ASPSnippets.GoogleAPI;
using CodeExam.Models;
using CodeExam.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                    var userName = User.Identity.Name;
                    var obj = db.Users.FirstOrDefault(d => d.UserName.Equals(userName));
                    return RedirectToLocal(ReturnUrl, obj.RoleId);
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
                        ViewBag.Notification = "Đăng nhập sai tên tài khoản hoặc mật khẩu";
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
                    return RedirectToAction("Index", "Direction");
                }
                User user = new User();
                user.DisplayName = profile.DisplayName;
                user.Email = profile.Emails.Find(e => e.Type == "account").Value;
                user.SocialId = profile.Id;
                user.RoleId = (int)RoleCommon.User;
                db.Users.Add(user);
                db.SaveChanges();
                var currentUser = db.Users.Where(u => u.SocialId == profile.Id).FirstOrDefault();
                SignInUser(currentUser.UserName, currentUser.Password, currentUser.SocialId, currentUser.RoleId, false);
                return RedirectToAction("Index", "Direction");
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
                return RedirectToAction("Index", "Direction");
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

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(AccountViewModel acc)
        {
            if (String.IsNullOrEmpty(acc.Email))
            {
                ViewBag.Message = "Nhập vào email";
            }
            else
            {
                var obj = db.Users.FirstOrDefault(u => u.Email == acc.Email);
                if (obj != null)
                {
                    acc.ActiveCode = Guid.NewGuid().ToString();
                    obj.ActiveCode = Encryption.Encrypt(acc.ActiveCode);
                    db.SaveChanges();
                    SendEmail(obj.Email, acc.ActiveCode.ToString(), Request.Url);
                    ViewBag.Message = "Hệ thống đã gửi email xác nhận cho bạn";
                }
                else
                {
                    ViewBag.Message = "Email không tồn tại";
                }
            }
            return View(acc);
        }

        public void SendEmail(string email, string activeCode, Uri uri)
        {
            var verifyUrl = "/Login/Resetpassword?activeCode=" + activeCode;
            var link = uri.Authority + uri.AbsolutePath.Replace(uri.AbsolutePath, verifyUrl);
            string Email = ConfigurationManager.AppSettings["Email"];
            var fromEmail = new MailAddress(Email, "Testttt");
            string fromEmailPw = ConfigurationManager.AppSettings["Password"];
            var toMail = new MailAddress(email);
            string subject = "";
            string body = "";
                subject = "Reset Password";
                body = "Click vào link để kích hoạt reset mật khẩu cho tài khoản: <a href='" + link + "'>" + link + "</a>";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPw)
            };
            using (var mess = new MailMessage(fromEmail, toMail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(mess);
        }

        [HttpGet]
        public ActionResult ResetPassword(string activeCode)
        {
            activeCode = Encryption.Encrypt(activeCode);
            var obj = db.Users.FirstOrDefault(u => u.ActiveCode == activeCode);
            AccountViewModel acc = new AccountViewModel();
            acc.ID = obj.UserId;
            acc.ActiveCode = obj.ActiveCode;
            acc.DisplayName = obj.DisplayName;
            return View(acc);
        }

        [HttpPost]
        public ActionResult ResetPassword(AccountViewModel acc)
        {
            if (acc.Password == acc.RePassword)
            {
                var obj = db.Users.FirstOrDefault(u => u.UserId == acc.ID);
                obj.Password = Encryption.Encrypt(acc.Password);
                db.SaveChanges();
                return RedirectToAction("/Index");
            }
            else
            {
                ViewBag.Message = "Mật khẩu không khớp";
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountViewModel acc)
        {
            if (String.IsNullOrEmpty(acc.UserName) || String.IsNullOrEmpty(acc.DisplayName) || String.IsNullOrEmpty(acc.Password) || String.IsNullOrEmpty(acc.RePassword))
            {
                ViewBag.Message = "Yêu cầu điền đầy đủ thông tin";
                return View();
            }
            if (db.Users.Where(d => d.UserName == acc.UserName).Count() > 0)
            {
                ViewBag.Message = "Tên tài khoản đã tồn tại";
                return View();
            }
            if (acc.Password != acc.RePassword)
            {
                ViewBag.Message = "Mật khẩu không khớp";
                return View();
            }
            if (db.Users.Where(d => d.Email == acc.Email).Count() > 0)
            {
                ViewBag.Message = "Email đã được sử dụng";
                return View();
            }
            User obj = new User();
            obj.UserName = acc.UserName;
            obj.Password = Encryption.Encrypt(acc.Password);
            obj.Email = acc.Email;
            obj.DisplayName = acc.DisplayName;
            obj.RoleId = (int)RoleCommon.User;
            obj.UserStatus = 1;

            db.Users.Add(obj);
            db.SaveChanges();
            return RedirectToAction("/");
        }
    }
}