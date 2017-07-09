using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using EadProjectShoppingApp.Models;
using EadProjectShoppingApp.Models.Extended;

namespace EadProjectShoppingApp.Controllers
{
    public class AdminController : Controller
    {
        
        //login
        [AllowAnonymous]
        [HttpGet]

        public ActionResult LoginAdmin()
        {
            if (System.Web.HttpContext.Current.Session["username"] != null)
            {
                return RedirectToAction("AdminView", "Admin", new { username = Session["username"].ToString() });

            }
            return View();




        }


        //login post action
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public ActionResult LoginAdmin(UserLogin login, string returnUrl)
        {
            //string message = "";
            //using (Shopping_Store_DataBaseEntities sd = new Shopping_Store_DataBaseEntities())
            //{
            //    var v = sd.Users.Where(a => a.EmailId == login.EmailId).FirstOrDefault();
            //    if (v != null)
            //    {
            //        if (string.Compare(Crypto.Hash(login.Password),v.Password)==0)
            //        {
            //            // if user has cehecked remember me option then save password for 1 year 525600 min= 1 year otherwise 20 minute
            //            int timeout = login.RememberMe ? 525600 : 20;
            //            var ticket=new FormsAuthenticationTicket(login.EmailId,login.RememberMe,timeout);
            //            string encrypted = FormsAuthentication.Encrypt(ticket);
            //            var cookie= new HttpCookie(FormsAuthentication.FormsCookieName,encrypted);
            //            cookie.Expires = DateTime.Now.AddMinutes(20);
            //            cookie.HttpOnly = true;
            //            Response.Cookies.Add(cookie);

            //            if (Url.IsLocalUrl(returnUrl))
            //            {
            //                return Redirect(returnUrl);
            //            }
            //            else
            //            {
            //                return RedirectToAction("Index", "Home");

            //            }

            //        }
            //        else
            //        {
            //            message = "Invalid credential provided";

            //        }

            //    }
            //    else
            //    {
            //        message = "Invalid credential provided";

            //    }
            //}
            //    ViewBag.Message = message;


            Shopping_Store_DataBaseEntities db = new Shopping_Store_DataBaseEntities();
            var v = db.Users.SingleOrDefault(x => x.EmailId == login.EmailId);
            //        if (string.Compare(Crypto.Hash(login.Password),v.Password)==0)

            if (v != null && String.CompareOrdinal(Crypto.Hash(login.Password), v.Password) == 0)
            {
                ViewBag.message = "Login";
                ViewBag.triedOnce = "yes";
                System.Web.HttpContext.Current.Session["username"] = v.EmailId;
                return RedirectToAction("AdminView", "Admin", new { username = login.EmailId });

            }

        
                ViewBag.triedOnce = "yes";
                return View();

            


        }

        //logout
        //[Authorize]
        //public ActionResult LogOut()
        //{
        //    Session.Abandon();
        //    FormsAuthentication.SignOut();
        //    //Session.Abandon();
        //    return RedirectToAction("Login", "Users");
        //}

        [HttpGet]
        public ActionResult LogOut()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();

            return RedirectToAction("Index","Home");
        }




        //for validation of email

        [NonAction]
        public bool IsEmailExist(String emailId)
        {
            using (Shopping_Store_DataBaseEntities sd = new Shopping_Store_DataBaseEntities())
            {
                var v = sd.Users.FirstOrDefault(a => a.EmailId == emailId);
                return v != null;
            }
        }

        // for sending email 

        [NonAction]

        public void SendVeificationLinkEmail(string emailId, string activationCode)
        {
            var verifyUrl = "/Users/VerifyAccount/" + activationCode;
            if (Request.Url != null)
            {
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                var fromEmail = new MailAddress("ammarakhalid126@gmail.com", "Ammara");

                var toEmail = new MailAddress(emailId);

                var fromEmailPassword = "ammara45???";

                string subject = "Your account is successfully created";

                string body =
                    "<br/><br/>We are excited to tell you that your online mobile shopping account sucessfully created.Please Click on the below link to verify your account <br/><br/><a href='" +
                    link + "' >" + link + "</a>";
                // configure smtp client for sending email
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

                };

                using (var message = new MailMessage(fromEmail, toEmail)
                    {
                        Subject = subject,
                        Body = body,
                        //IsBodyHtml=true bcz i use html tags in body
                        IsBodyHtml = true
                    })
                    //to send email
                    smtp.Send(message);




            }



        }

        public ActionResult AdminView(string username)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
           
                Shopping_Store_DataBaseEntities db= new Shopping_Store_DataBaseEntities();
                ViewBag.username = Session["username"];
                return View(db.Categories.ToList());
            
        }

        
    }
}