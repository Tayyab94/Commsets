using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLibrary;
using System.Web.Mvc;
using finalProject.Models;
using BusinessLibrary.UserManagement;
using BusinessLibrary.Classified;
using System.Web.Hosting;
using System.Text;
using System.Net.Mail;
using System.Diagnostics;
using System.Data.Entity;
using System.Net;

namespace finalProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            ViewBag.ReturnUrl = Request.QueryString["returnurl"];  //Query String 

            HttpCookie mycookie = Request.Cookies[WebUtil.USER_COOKIEE];
            if (mycookie != null)
            {
                mycookie.Expires = DateTime.Today.AddDays(7);


                Response.SetCookie(mycookie);
                string[] data = mycookie.Value.Split(',');
                User user = new UserHandler().getLoginUser(data[0], data[1]);
                if (user != null)
                {
                    Session.Add(WebUtil.CURRENT_USER, user);
                    if (user.IsRole(WebUtil.ADMIN_ROLE))
                    {
                        return RedirectToAction("Index", new { area = "AdminSide", Controller = "Home" });
                    }
                    return RedirectToAction("Index", "Home1");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel obj)
        {
            User objUSer = new UserHandler().getLoginUser(obj.LoginId, obj.Password);
            if (objUSer != null)
            {
                Session.Add(WebUtil.CURRENT_USER, objUSer);
                if (obj.RememberMe)
                {
                    if (Request.Browser.Cookies)
                    {
                        HttpCookie c = new HttpCookie(WebUtil.USER_COOKIEE);
                        c.Expires = DateTime.Today.AddDays(7);
                        c.Value = $"{objUSer.LoginId},{objUSer.Password}";
                        Response.SetCookie(c);
                    }
                }

                string temp = Request.QueryString["returnurl"];
                if (objUSer.IsRole(WebUtil.ADMIN_ROLE))
                {
                    if (!string.IsNullOrWhiteSpace(temp))
                    {
                        string[] parts = temp.Split('/');
                        return RedirectToAction(parts[1], parts[0]);
                    }
                    return RedirectToAction("Index", new { area = "AdminSide", Controller = "Home" });
                }

                if (!string.IsNullOrWhiteSpace(temp))
                {
                    string[] parts = temp.Split('/');
                    return RedirectToAction(parts[1], parts[0]);
                }
                return RedirectToAction("Index", "Home1");

                //if(objUSer.IsRole(WebUtil.ADMIN_ROLE))
                //{
                //    if (!string.IsNullOrWhiteSpace(temp)) ;
                //    return RedirectToAction("ShowRegisterUsers", "User");
                //}
                //if (!string.IsNullOrWhiteSpace(temp)) return RedirectToAction(parts[1], parts[0]);
                //return RedirectToAction("PostAdvertisement", "Advertisement");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpCookie mycookie = Request.Cookies[WebUtil.USER_COOKIEE];
            if (mycookie != null)
            {
                mycookie.Expires = DateTime.Now;
                Response.SetCookie(mycookie);
            }
            Session.Abandon();
            return RedirectToAction("Index", "Home1");
        }

        [HttpGet]
        public ActionResult SignIn()
        {
           
            ViewBag.Cities = new LocationHandler().getAllCities().ToSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(FormCollection data)
        {
            try
            {

                DemoContext _context = new DemoContext();
                User objUSer = new User();

                objUSer.IsActive = false;
                objUSer.FullName = data["fulName"];
                objUSer.LoginId = data["loginID"];
                objUSer.Password = data["pasword"];
                objUSer.SecurityQuestion = data["SecurityQ"];
                objUSer.SecurityAns = data["SecurityA"];
                objUSer.ContactNo = data["contactNo"];
                objUSer.Address = data["address"];
                objUSer.EmailID = data["EmailID"];
                objUSer.DOB = Convert.ToDateTime(data["Dob"]);
                objUSer.City = new City { ID = Convert.ToInt32(data["cityId"]) };

                objUSer.Role = new Role { ID = 1 };

                long uno = DateTime.Now.Ticks;
                int count = 0;
                foreach (string fcName in Request.Files)
                {

                    HttpPostedFileBase file = Request.Files[fcName];
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        string url = "/Images/U_profileImages/" + $"{uno}_{++count}" + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        objUSer.ImgURL = url;

                    }
                }
                new UserHandler().Add(objUSer);
                BuildEmailTemplate(objUSer.ID);
                TempData.Add("AlertMessage", new AlertModel("Check your Email, We have sent the confirmation Mail ", AlertType.Success));

            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Something is wrong! ", AlertType.Error));
                Trace.WriteLine(DateTime.Now.ToString("F") + "," + ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("____________________________________________");
                Trace.Flush();
            }
            ModelState.Clear();// bus itni c bat thi :P

            return RedirectToAction("SignIn");
        }

        //Metod For Eamil Notificatiaon 
        public void BuildEmailTemplate(int regID)
        {
            DemoContext _Context = new DemoContext();
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = _Context.Users.Where(x => x.ID == regID).FirstOrDefault();

            var url = "http://localhost:56386/" + "User/Confirm?regId=" + regID;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your Account is Successfully Created", body, regInfo.EmailID);
        }
        //  This one is also for the email notification 
        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "tayyab.bhatti30@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }

            if (!string.IsNullOrEmpty(cc))
            {

                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        //This method  work for sending the email ss
        private static void SendEmail(MailMessage mail)  //This Methos work for Send Built Email
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("Email", "Password");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Confirmation Method that would be run at that time when the new User regustered youself and check the mail of  confirmation 
        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }

        public JsonResult RegisterConfirm(int regId)  //This Method is work for Active usr account that mean it save isValid= true
        {                                                      //in the database and return a Msg Account is activated Now 

            DemoContext _context = new DemoContext();
            User objUser = _context.Users.Where(x => x.ID.Equals(regId)).FirstOrDefault();
            objUser.IsActive = true;
            _context.SaveChanges();
            var msg = "Your Message is Varified";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ShowRegisterUsers()
        {
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "User/ShowRegisterUsers" });

            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                List<User> UsersList = new UserHandler().getAllUsers().ToList();
                return View(UsersList);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //return RedirectToAction("Login", "User");
            }

        }

        //Delete user Method

        [HttpGet]

        public ActionResult DeleteUser(int id)
        {
            //DemoContext _context = new DemoContext();
            //User obj = _context.Users.Where(x => x.ID.Equals(id)).SingleOrDefault();
            //_context.Users.Remove(obj);
            //_context.SaveChanges();
            //return RedirectToAction("ShowRegisterUsers","User");

            DemoContext _context = new DemoContext();
            User obj = _context.Users.Where(x => x.ID == id).SingleOrDefault();
            List<Advertisement> Ad = _context.Advertisements.Include(a => a.City.Province.Country).Include(a => a.Status).Include(a => a.Images)
                  .Include(a => a.SubCatagory.Catagory).Include(a => a.Type).Include(a => a.User).Where(x => x.User.ID.Equals(obj.ID)).ToList();
            foreach (Advertisement item in Ad)
            {
                foreach (var c in item.Images.ToList())
                {
                    string path = Request.MapPath("~/" + c.Url);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    if (path != null)
                    {
                        _context.Entry(c).State = System.Data.Entity.EntityState.Deleted;
                        _context.SaveChanges();
                    }

                    _context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    _context.SaveChanges();
                }
            }
            _context.Users.Remove(obj);
            _context.SaveChanges();
            return RedirectToAction("ShowRegisterUsers");
        }
        public ActionResult UserProfile(int id)
        {
            DemoContext _context = new DemoContext();
            User objUser = new UserHandler().getUserById(id);
            // ViewBag.ListAdv= new AdvertisementHandler().GetUserAdvertisement(objUser.ID).ToAdvertiseSumModel();
            List<Advertisement> uAdvertisementList = new AdvertisementHandler().GetUserAdvertisement(objUser.ID).ToList();
            ViewBag.UAdvList = uAdvertisementList;

            var showpenddinglist = _context.Advertisements.Include(x => x.City.Province.Country)
                .Include(x => x.Images).Include(x => x.Status).Include(x => x.SubCatagory.Catagory)
                .Include(x => x.Type).Include(x => x.User)
                .Where(x => x.User.ID==id && x.Status.ID == 3)
                .ToList();
            ViewBag.penddinglist = showpenddinglist;
            return View(objUser);
        }



        [HttpGet]
        public ActionResult EditUser(int id)
        {
            DemoContext _context = new DemoContext();
            User objUser = new UserHandler().getUserById(id);
            ViewBag.usrCityList = new SelectList(_context.Ciities, "ID", "Name", objUser.City.ID);
            return View(objUser);
        }

        [HttpPost]
        public ActionResult EditUser(User obj)
        {
            try
            {
                DemoContext _context = new DemoContext();
                User user = _context.Users.Where(x => x.ID.Equals(obj.ID)).FirstOrDefault();
                user.FullName = obj.FullName;
                user.EmailID = obj.EmailID;
                user.LoginId = obj.LoginId;
                user.Password = obj.Password;
                user.SecurityQuestion = obj.SecurityQuestion;
                user.SecurityAns = obj.SecurityAns;
                user.ContactNo = obj.ContactNo;
                user.Address = obj.Address;
                user.DOB = Convert.ToDateTime(obj.DOB);
                user.City = new City { ID = Convert.ToInt32(obj.City.ID) };
                _context.Entry(user.City).State = System.Data.Entity.EntityState.Unchanged;
                long uno = DateTime.Now.Ticks;
                int count = 0;
                foreach (string fcName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fcName];
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        string url = "/Images/U_profileImages/ " + $"{uno}_{++count}" + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        user.ImgURL = url;

                    }
                }
                _context.SaveChanges();
                TempData.Add("userUpdateAlert", new AlertModel("Updation Successfully....", AlertType.Success));
                ViewData["UpdateMessageAlert"] = "Data is Updated successfully..!";
            }
            catch
            {
                TempData.Add("userUpdateAlert", new AlertModel("Sorry Something is Wrong Please try Again...!", AlertType.Error));
                return RedirectToAction("EditUser");
            }

            return RedirectToAction("ShowRegisterUsers");
        }

        [HttpGet]
        public ActionResult ChangePass(int id)
        {
            User objUser = new UserHandler().getUserById(id);
            ViewBag.UserId = objUser.ID;
            ChangePasswordModel passChangeModel = new ChangePasswordModel
            {
                UserId = objUser.ID,
                // CurrentPass = objUser.Password
            };
            return View(passChangeModel);
        }


        [HttpPost]
        public ActionResult ChangePass(ChangePasswordModel data, int id)
        {
            // mazak na kr strongly bind :D 
            DemoContext _context = new DemoContext();
            User objUser = new UserHandler().getUserById(data.UserId);


            // wait idiot, keys galat hay tari , is ko run kr 

            /*= new UserHandler().getUserById(id);*/
            string pa = objUser.Password;
            if (ModelState.IsValid)
            {
                if (pa.Equals(data.CurrentPass))
                {
                    objUser.Password = data.NewPass;
                    // repeat kr ley.
                    _context.Entry(objUser).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    TempData.Add("PasUpdateAlert", new AlertModel("your Password have been changed  Successfully..!", AlertType.Success));
                }
                else
                {
                    TempData.Add("PasUpdateAlert", new AlertModel("Please Confirm your Current Password!", AlertType.Warning));
                }
            }
            return View();
        }





        //[HttpPost]
        //public ActionResult ChangePass(FormCollection data)
        //{
        //    // mazak na kr strongly bind :D 
        //    DemoContext _context = new DemoContext();
        //    User objUser = new UserHandler().getUserById(Convert.ToInt32(data["UserId"]));
        //    if (ModelState.IsValid)
        //    {

        //        // wait idiot, keys galat hay tari , is ko run kr 

        //        /*= new UserHandler().getUserById(id);*/
        //        string pa = objUser.Password;
        //        string CurrentPas = null; /*= data["pasword"];*/
        //        if (pa.Equals(data["CurrentPass"]))
        //        {
        //            User objUser1 = _context.Users.Where(x => x.ID.Equals(id)).SingleOrDefault();
        //            objUser1.Password = data["NewPass"];
        //            _context.SaveChanges();
        //            TempData.Add("PasUpdateAlert", new AlertModel("your Password have been changed  Successfully..!", AlertType.Success));
        //        }
        //        else
        //        {
        //            TempData.Add("PasUpdateAlert", new AlertModel("your Current Password is Wrong..!", AlertType.Success));
        //        }
        //    }

        //    //User objUser = new UserHandler().getUserById(id);
        //    return View(objUser);
        //}

        public ActionResult forgetPass()
        {
            return View();
        }


        [HttpPost]
        public ActionResult forgetPass(ChangePasswordModel obj)
        {
            DemoContext _context = new DemoContext();
            User objUser = _context.Users.SingleOrDefault(x => x.EmailID.Equals(obj.CurrentEmail));
            if (objUser != null)
            {
                Random R = new Random();
                //  string num = R.Next(0, 100000).ToString("D6");
                //Guid R1 = Guid.NewGuid();

                int Rend = R.Next(100, 1000);
                Session["GenerateNo"] = Rend;
                // TempData["GenerateNo"] = R;
                // TempData.Keep();
                //ViewBag.Gen = Rend;

                //  User userobj = _context.Users.Where(x => x.ID.Equals(objUser.ID)).SingleOrDefault();

                objUser.VerificationCode = Session["GenerateNo"].ToString();
                _context.SaveChanges();


                BuildEmailTemplate1(objUser.ID);
                //  TempData["GenerateNo"] = null;
                return RedirectToAction("ForGettingPass", "User");

            }

            return View();
        }
        [HttpGet]
        public ActionResult ForGettingPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForGettingPass(ForgetLoginModel obj)
        {
            DemoContext _context = new DemoContext();
            User objUser = new User();
            try
            {
                objUser = _context.Users.Where(x => x.VerificationCode.Equals(obj.VerificationCode)).SingleOrDefault();

                objUser.Password = Convert.ToString(obj.NewPass);
                _context.Entry(objUser).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                TempData.Add("PasUpdateAlert", new AlertModel("your Password have been Updated Successfully..!", AlertType.Success));

            }
            catch
            {
                TempData.Add("PasUpdateAlert", new AlertModel("Please Confirm Your Verification first", AlertType.Error));
            }

            return View();
        }

        public void BuildEmailTemplate1(int regID)
        {
            DemoContext _Context = new DemoContext();
            //int number = Convert.ToInt32(TempData["GenerateNo"]);
            string body = "This is your verification code " + Session["GenerateNo"];
            //string  body=System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Views/User/") + "ForeGetMail1" + ".cshtml");
            var regInfo = _Context.Users.Where(x => x.ID == regID).FirstOrDefault();


            body = body.ToString();
            BuildEmailTemplate1("Verification Code for Recovery Account", body, regInfo.EmailID);
        }
        //  This one is also for the email notification 
        public static void BuildEmailTemplate1(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "tayyab.bhatti30@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }

            if (!string.IsNullOrEmpty(cc))
            {

                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail1(mail);

        }

        //This method  work for sending the email ss
        private static void SendEmail1(MailMessage mail)  //This Methos work for Send Built Email
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("Email", "Password");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public ActionResult userDetai(int id)
        {
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "User/userDetai" });

            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                DemoContext _context = new DemoContext();
                User objUser = new UserHandler().getUserById(id);
                return View(objUser);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


    }
}