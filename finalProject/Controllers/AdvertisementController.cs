using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLibrary;
using finalProject.Models;
using BusinessLibrary.Classified;
using System.Data.Entity;
using BusinessLibrary.UserManagement;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Web.Hosting;

namespace finalProject.Controllers
{
    public class AdvertisementController : Controller
    {
        // GET: Advertisement
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult PostAdvertisement()
        {
            //Query String Method
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Advertisement/PostAdvertisement" });

            ViewBag.CountryList = new LocationHandler().getAllCountries().ToSelectList();
            ViewBag.CatagoryList = new CatagoryHendler().getAllCatagories().ToSelectList();
            var type = new AdvertisementTypeHandler().GetAllAdvertisementTypes().ToSelectList();
            type.First().Selected = true;
            ViewBag.advType = type;
            return View();
        }

        //Method for posting the  advertisement 
        [HttpPost]
        public ActionResult PostAdvertisement(FormCollection data)
        {
            DemoContext _context = new DemoContext();
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Advertisement/PostAdvertisement" });
            if (currentUser.IsActive == false)
            {
                TempData.Add("PostAlertMessage", new AlertModel("Pleae Activate your Account Before the Posting Something Here....!", AlertType.Error));
                return RedirectToAction("PostAdvertisement");
            }
            try
            {

                Advertisement objAdv = new Advertisement();
                objAdv.City = new City { ID = Convert.ToInt32(data["City"]) };
                _context.Entry(objAdv.City).State = System.Data.Entity.EntityState.Unchanged;
                objAdv.Title = data["advTitle"];
                objAdv.price = Convert.ToSingle(data["advPrice"]);
                objAdv.VilidUpTo = Convert.ToDateTime(data["advValidity"]);
                objAdv.SubCatagory = new SubCatagory { ID = Convert.ToInt32(data["SubCatagory"]) };
                _context.Entry(objAdv.SubCatagory).State = System.Data.Entity.EntityState.Unchanged;
                objAdv.Type = new AdvertisementType { ID = Convert.ToInt32(data["advTpye"]) };
                _context.Entry(objAdv.Type).State = System.Data.Entity.EntityState.Unchanged;
                objAdv.Description = data["Description"];
                objAdv.Status = new AdvertisementStatus { ID = 2 };
                _context.Entry(objAdv.Status).State = System.Data.Entity.EntityState.Unchanged;
                objAdv.User = new User { ID = currentUser.ID };
                _context.Entry(objAdv.User).State = System.Data.Entity.EntityState.Unchanged;
                objAdv.PostedOn = DateTime.Now;
                long uno = DateTime.Now.Ticks;
                int count = 0;
                foreach (string file_Name in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[file_Name];
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        string url = "/Images/PostAdvImages/" + $"{uno}_{++count}" + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        string path = Server.MapPath(url);
                        file.SaveAs(path);
                        objAdv.Images.Add(new AdvertisementImages { Url = url, Priority = count });
                    }
                }
                //new AdvertisementHandler().Add(objAdv);
                _context.Advertisements.Add(objAdv);
                _context.SaveChanges();
                TempData.Add("PostAlertMessage", new AlertModel("your Post is Successfully Uploaded..!", AlertType.Success));
            }
            catch
            {
                TempData.Add("PostAlertMessage", new AlertModel("Something Wrong with thi Please fill your form carefully...!", AlertType.Error));
            }


            //return RedirectToAction("Index","Home1");
            return RedirectToAction("PostAdvertisement");
        }


        //Detail Post Method
        [HttpGet]
        public ActionResult DetailProduct(int id)
        {
            DemoContext db = new DemoContext();
            //var product= db.Advertisements.Include("AdvertisementImages").Where(x => x.ID == id).FirstOrDefault();
            Advertisement objAdvertisement = new AdvertisementHandler().GetDetailAdvertisement(id);
            //        AdvSumModel tem = new AdvSumModel();
            //tem.Id = objAdvertisement.ID;
            //tem.ImgUrl = (objAdvertisement.Images.Count > 0) ? objAdvertisement.Images.First().Url : "/Images/temp/nophoto.png";


            //Function for getting the Recommended AdvertisementProduct
            ViewBag.recomendedAdvertisement = new AdvertisementHandler().GetRecomendAdvertisement(objAdvertisement.SubCatagory.Catagory.ID).ToAdvertiseSumModel();
            return View(objAdvertisement);
        }

        ////delete post method 
        //[httpget]
        //public actionresult deteleadv(int id)
        //{
        //    democontext _context = new democontext();
        //    advertisement objadv = new advertisementhandler().getdetailadvertisement(id);
        //    foreach (var child in objadv.images.tolist())
        //    {
        //        _context.entry(child).state = system.data.entity.entitystate.deleted;
        //        _context.savechanges();
        //    }
        //    _context.advertisements.remove(objadv);
        //    _context.savechanges();

        //    return redirecttoaction("index", "home1");
        //}

        //delete post method 
        [HttpGet]
        public ActionResult deteleadv(int id)
        {
            DemoContext _context = new DemoContext();
            Advertisement objadv = new AdvertisementHandler().GetDetailAdvertisement(id);
            foreach (var child in objadv.Images.ToList())
            {
                String PATH = Request.MapPath("~/" + child.Url);
                if (System.IO.File.Exists(PATH))
                {
                    System.IO.File.Delete(PATH);
                }
                if (PATH != null)
                {
                    _context.Entry(child).State = System.Data.Entity.EntityState.Deleted;
                    _context.SaveChanges();
                }

            }

            _context.Entry(objadv).State = System.Data.Entity.EntityState.Deleted;
           // _context.Advertisements.Remove(objadv);
            _context.SaveChanges();

            return RedirectToAction("index", "home1");
        }

        public ActionResult EditPost(int id)
        {
            DemoContext _context = new DemoContext();
            Advertisement objAdv = new AdvertisementHandler().GetDetailAdvertisement(id);
            var type = new AdvertisementTypeHandler().GetAllAdvertisementTypes().ToSelectList();
            // type.Find(x => Convert.ToInt32(x.Value)==objAdv.Type.ID).Selected = true;
            ViewBag.advType = type;

            //This code is  select the selected Country of the Post
            ViewBag.CountryList = new SelectList(_context.Countries, "ID", "Name", objAdv.City.Province.Country.ID);

            //This code is  select the selected Province of the Post
            List<SelectListItem> prv = ModelHelper.ToSelectList(new LocationHandler().getProvincesBtCountry(objAdv.City.Province.Country));
            prv.Find(x => Convert.ToInt32(x.Value) == objAdv.City.Province.ID).Selected = true;
            ViewBag.ProvinceList = prv;


            //This code is  select the selected Cities of the Post
            var citi = ModelHelper.ToSelectList(_context.Ciities.OrderBy(c => c.Name).Where(x => x.Province.ID.Equals(objAdv.City.Province.ID)).ToList());
            // ViewBag.ciList = citi;

            var s = new SelectList(_context.Ciities.OrderBy(x => x.Name).Where(x => x.Province.ID == objAdv.City.Province.ID).ToList(), "ID", "Name");
            ViewBag.ciList = s;
            //This code is also select the selected Catagory of the post
            ViewBag.CatagoryList = new SelectList(_context.Catagories.OrderBy(x => x.Name), "ID", "Name", objAdv.SubCatagory.Catagory.ID);

            //This code is  select the selected SubCatagory of the post
            List<SelectListItem> subcats = ModelHelper.ToSelectList(new CatagoryHendler().getSubCatagoriesByCatagoryId(objAdv.SubCatagory.Catagory));
            subcats.Find(x => Convert.ToInt32(x.Value) == objAdv.SubCatagory.ID).Selected = true;
            ViewBag.SubCatagoryList = subcats;
            // ViewBag.SubCatagoryList = new SelectList(_context.SubCatagories.OrderBy(x => x.Name), "ID", "Name", objAdv.SubCatagory.ID);

            return View(objAdv);
        }
        [HttpPost]
        public ActionResult EditPost(Advertisement objAdv, FormCollection data)
        {
            DemoContext _context = new DemoContext();
            User currentUser = (User)Session[WebUtil.CURRENT_USER];

            Advertisement objAdv1 = new AdvertisementHandler().GetDetailAdvertisement(objAdv.ID);
            objAdv1.City = new City { ID = Convert.ToInt32(data["CityId"]) };
            _context.Entry(objAdv1.City).State = System.Data.Entity.EntityState.Unchanged;
            objAdv1.Title = data["Title"];
            objAdv1.price = Convert.ToSingle(data["Price"]);
            objAdv1.VilidUpTo = Convert.ToDateTime(data["ValidUpTO"]);
            objAdv1.SubCatagory = new SubCatagory { ID = Convert.ToInt32(data["SubCatagoryID"]) };
            _context.Entry(objAdv1.SubCatagory).State = System.Data.Entity.EntityState.Unchanged;
            objAdv1.Type = new AdvertisementType { ID = Convert.ToInt32(data["advTpye"]) };
            _context.Entry(objAdv1.Type).State = System.Data.Entity.EntityState.Unchanged;
            objAdv1.Description = data["Descrip"];
            objAdv1.Status = new AdvertisementStatus { ID = 2 };
            _context.Entry(objAdv1.Status).State = System.Data.Entity.EntityState.Unchanged;

            objAdv1.User = new User { ID = currentUser.ID };
            _context.Entry(objAdv1.User).State = System.Data.Entity.EntityState.Unchanged;


            //THis Method is for geleter the Images form the table
            foreach (var c in objAdv1.Images.ToList())
            {
                string Path = Request.MapPath("~/" + c.Url);
                if(System.IO.File.Exists(Path))
                {
                    System.IO.File.Delete(Path);
                }
                _context.Entry(c).State = System.Data.Entity.EntityState.Deleted;
                _context.SaveChanges();
            }
            objAdv1.PostedOn = DateTime.Now;
            long uno = DateTime.Now.Ticks;
            int count = 0;
            foreach (string file_Name in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[file_Name];
                if (!string.IsNullOrWhiteSpace(file.FileName))
                {
                    string url = "/Images/PostAdvImages/" + $"{uno}_{++count}" + file.FileName.Substring(file.FileName.LastIndexOf("."));
                    string path = Server.MapPath(url);
                    file.SaveAs(path);
                    objAdv1.Images.Add(new AdvertisementImages { Url = url, Priority = count });

                }
            }
            //_context.Entry(objAdv1).State = System.Data.Entity.EntityState.Modified;
            _context.Advertisements.Add(objAdv1);
          

            return RedirectToAction("UserProfile", "User", new { id = currentUser.ID });
        }


        [HttpGet]
        public ActionResult ApproavedPostStatus(int id)
        {
            DemoContext _context = new DemoContext();
            try
            {
                Advertisement objAdv = _context.Advertisements.Include(x => x.City.Province.Country)
               .Include(x => x.Images).Include(x => x.Status).Include(x => x.SubCatagory.Catagory).Include(x => x.Type).Include(x => x.User).Where(x => x.ID == id).SingleOrDefault();
                objAdv.Status = new AdvertisementStatus { ID = 1 };
                _context.Entry(objAdv.Status).State = System.Data.Entity.EntityState.Unchanged;
                _context.Entry(objAdv).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                TempData.Add("ApproavedStatusPost", new AlertModel("Post have been Approaved Successfully...!", AlertType.Success));
            }
            catch
            {
                TempData.Add("ApproavedStatusPost", new AlertModel("Something is wrong please try again...!", AlertType.Error));
            }

            return RedirectToAction("PenddingPost", new { area = "AdminSide", Controller = "Home" });
        }

        [HttpGet]
        public ActionResult rejectPostStatus(int id)
        {
            DemoContext _context = new DemoContext();
            Advertisement objAvd = _context.Advertisements.Include(x => x.City.Province.Country).Include(x => x.Images).Include(x => x.Status).Include(x => x.SubCatagory.Catagory).Include(x => x.Type).Include(x => x.User).Where(x => x.ID.Equals(id)).SingleOrDefault();
            objAvd.Status = new AdvertisementStatus { ID = 3 };

            _context.Entry(objAvd.Status).State = System.Data.Entity.EntityState.Unchanged;
            _context.Entry(objAvd).State = System.Data.Entity.EntityState.Modified;
         

            BuildEmailTemplate(objAvd.ID);
            _context.SaveChanges();
            return RedirectToAction("PenddingPost", new { area = "AdminSide", Controller = "Home" });
        }



        //Metod For Eamil Notificatiaon 
        public void BuildEmailTemplate(int regID)
        {
            DemoContext _context = new DemoContext();
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "ForeGetMail" + ".cshtml");
            var regInfo = _context.Advertisements.Include(x => x.City.Province.Country).Include(x => x.Images).Include(x => x.Status).Include(x => x.SubCatagory.Catagory).Include(x => x.Type).Include(x => x.User).Where(x => x.ID.Equals(regID)).SingleOrDefault();
            var url = "http://localhost:56386/" + "User/Login";
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Email is Send successfully", body, regInfo.User.EmailID);
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
            client.Credentials = new System.Net.NetworkCredential("tayyab.bhatti30@gmail.com", "@@@123456789T");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }

}