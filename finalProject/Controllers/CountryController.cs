using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessLibrary;
using BusinessLibrary.UserManagement;
using PagedList;

namespace finalProject.Controllers
{
    public class CountryController : Controller
    {
        DemoContext db = new DemoContext();
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCounty()
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Country/AddCounty" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult AddCounty(FormCollection data)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Country/AddCounty" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {

                Country country = new Country();
                country.Code = Convert.ToInt32(data["cCode"]);
                country.Name = data["cName"];
                db.Countries.Add(country);
                db.SaveChanges();
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult ShowCountry(int? page)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Country/ShowCountry" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {

                var pageNumber = page ?? 1;
                var pageSize = 5;
                var countryList = new LocationHandler().getAllCountries().ToPagedList(pageNumber, pageSize);
                ViewBag.ListCountry = countryList;
                return View(ViewBag.ListCountry);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Country/Edit" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {

                Country countryObj = new LocationHandler().getCountryById(id);
                ViewBag.country = countryObj;
                return View(ViewBag.country);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Edit(Country data)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Country/Edit" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {


                db.Entry<Country>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowCountry", "Country");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult Delete(int id)
        {
            Country country = db.Countries.Where(x => x.ID.Equals(id)).SingleOrDefault();
            db.Countries.Remove(country);

            db.SaveChanges();
            return RedirectToAction("ShowCountry", "Country");

        }
    }
}