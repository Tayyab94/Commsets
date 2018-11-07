using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessLibrary;
using BusinessLibrary.UserManagement;
using finalProject.Models;
using PagedList;


namespace finalProject.Controllers
{
    public class CityController : Controller
    {
        DemoContext _context = new DemoContext();
        // GET: City
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddCity()
        {
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "City/AddCity" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                ViewBag.provincesList = new LocationHandler().getAllPrivinces().ToSelectList();
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           
        }

        [HttpPost]
        public ActionResult AddCity(FormCollection data)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if(currentUser==null) return RedirectToAction("Login","User",new { returnurl="City/AddCity"});
            if(currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                City city = new City();
                city.Name = data["cityName"];
                city.Province = new Province { ID = Convert.ToInt32(data["ProvinceID"]) };
                _context.Entry(city.Province).State = System.Data.Entity.EntityState.Unchanged;
                _context.Ciities.Add(city);
                _context.SaveChanges();
                return RedirectToAction("ShowCities", "City");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
        }

        [HttpGet]
        public ActionResult ShowCities(int? page)
        {
            User currentUSer = (User)Session[WebUtil.CURRENT_USER];
            if (currentUSer == null) return RedirectToAction("Login", "User", new { returnurl = "City/ShowCities" });
            if(currentUSer.IsRole(WebUtil.ADMIN_ROLE))
            {
                var PageNumber = page ?? 1;
                var PageSize = 5;
                //List<City> citiesList = new LocationHandler().getAllCities().ToList();
                //var citiesList = _context.Ciities.Include("Province").OrderBy(x => x.Name).ToPagedList(PageNumber, PageSize);
                var citiesList = new LocationHandler().getAllCities().ToPagedList(PageNumber, PageSize);
                return View(citiesList);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
        }

       [HttpGet]
       public ActionResult EditCity(int id)
        {
            User currentUSer = (User)Session[WebUtil.CURRENT_USER];
            if (currentUSer == null) return RedirectToAction("Login", "User", new { returnurl = "City/EditCity" });
            if (currentUSer.IsRole(WebUtil.ADMIN_ROLE))
            {
                City city = new LocationHandler().getCityById(id);
            ViewBag.ProvinceList = new SelectList(_context.Provinces, "ID", "Name", city.Province.ID);
            return View(city);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult EditCity(City obj)
        {
            User currentUSer = (User)Session[WebUtil.CURRENT_USER];
            if (currentUSer == null) return RedirectToAction("Login", "User", new { returnurl = "City/EditCity" });
            if (currentUSer.IsRole(WebUtil.ADMIN_ROLE))
            {
                City city = _context.Ciities.Where(x => x.ID.Equals(obj.ID)).FirstOrDefault();
            city.Name = obj.Name;
            city.Province = new Province { ID = Convert.ToInt32(obj.Province.ID) };
            _context.Entry(city.Province).State = System.Data.Entity.EntityState.Unchanged;
            _context.SaveChanges();
            return RedirectToAction("ShowCities", "City");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult DeleteCity(int id)
        {
           // City city = new LocationHandler().getCityById(id);
            City cit = _context.Ciities.Where(x => x.ID.Equals(id)).SingleOrDefault();
            _context.Ciities.Remove(cit);
            _context.SaveChanges();
            return RedirectToAction("ShowCities", "City");
        }
    }
}