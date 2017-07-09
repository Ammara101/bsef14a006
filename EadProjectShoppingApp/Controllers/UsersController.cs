using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Mvc;
using EadProjectShoppingApp.Models;
using EadProjectShoppingApp.Models.Extended;

namespace EadProjectShoppingApp.Controllers
{
    public class UsersController : Controller
    {
        // registration action
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        //registration post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        //for automatic binding we use bind exclude
        public ActionResult Signup([Bind(Exclude = "IsEmailVerified,ActivationKey")] User user)
        {

            bool status = false;
            string message ;
            //model validation
            if (ModelState.IsValid)
            {

                #region //email is already exist or validation of email

                var isExist = IsEmailExist(user.EmailId);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }

                #endregion


                #region  generate activation key

                user.ActivationKey = Guid.NewGuid();

                #endregion

                //password hashing for security (not saving password in our db and hash value is stored in our db)

                #region

                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //to avoid confirm password validation issues

                #endregion

                //to avoid validation again on save changes i use 
                user.IsEmailVerified = false;

                #region save data in our database

                using (Shopping_Store_DataBaseEntities sd = new Shopping_Store_DataBaseEntities())
                {
                    user.IsAdmin = false;
                    sd.Users.Add(user);
                    sd.SaveChanges();

                    //send email to user
                    SendVeificationLinkEmail(user.EmailId, user.ActivationKey.ToString());
                    message =
                        "Registration is sucessfully done!.Account activation link has been send to your email id " +
                        user.EmailId;
                    status = true;

                }

                #endregion

            }
            else
            {
                message = "Invalid request";
            }





            ViewBag.Message = message;
            ViewBag.Status = status;




            return View(user);
        }
        //verify account

            // create a http get action for verify account using the activation code we have sent user email id when they have sucessfully submitted the registration page

        [HttpGet]
        
        public ActionResult VerifyAccount(String id) // from id i will get the activation code fromn the link which we have sent to the user email id this is means activation code
         {
            bool status = false;
            using (Shopping_Store_DataBaseEntities sd = new Shopping_Store_DataBaseEntities())
            {
                sd.Configuration.ValidateOnSaveEnabled = false; // this line i have added to avoid confirm password doesnot match issue

                var v=sd.Users.Where(a=>a.ActivationKey== new Guid(id)).FirstOrDefault();
                 
                // if link is valid then v is not null
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    sd.SaveChanges();
                    status = true;
                }
                else
                {
                    ViewBag.message = "Invalid request";

                }
            }
            ViewBag.Status = status;

            return View(); 

        }


        //login
        [HttpGet]

        public ActionResult Login()
        {
            if (System.Web.HttpContext.Current.Session["username"] != null)
            {
                return RedirectToAction("Cart","Users",new {username=Session["username"].ToString()});

            }
            return View();




        }


        //login post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login,string returnUrl)
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
             var v = db.Users.SingleOrDefault(x=>x.EmailId==login.EmailId  );
            //        if (string.Compare(Crypto.Hash(login.Password),v.Password)==0)

            if (v != null && String.CompareOrdinal(Crypto.Hash(login.Password), v.Password) == 0)
            {
                ViewBag.message = "Login";
                ViewBag.triedOnce = "yes";
                System.Web.HttpContext.Current.Session["username"] = v.EmailId;
                return RedirectToAction("Cart", "Users", new { username =login.EmailId });

            }

            else
            {
                ViewBag.triedOnce = "yes";
                  return View();

            }


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
          
            return RedirectToAction("Login");
        }




        //for validation of email

        [NonAction]
        public bool IsEmailExist(String emailId)
        {
            using (Shopping_Store_DataBaseEntities sd = new Shopping_Store_DataBaseEntities())
            {
                var v = sd.Users.Where(a => a.EmailId == emailId && a.ActivationKey==null).FirstOrDefault();
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

                var fromEmailPassword = "ammarabssef14";

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
        public ActionResult Cart(string username)
        {
            
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                ViewBag.username = Session["username"];
                return View();
            }
        }
    }
    
    
}