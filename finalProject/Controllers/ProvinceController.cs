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
    public class ProvinceController : Controller
    {
        private DemoContext _context = new DemoContext();
        // GET: Province
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddProvince()
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Province/AddProvince" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {

                ViewBag.countriesList = new SelectList(_context.Countries, "ID", "Name");
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // Add Province
        [HttpPost]
        public ActionResult AddProvince(FormCollection data)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Province/AddProvince" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                Province province = new Province();
            province.Name = data["provinceName"];
            province.Country = new Country { ID = Convert.ToInt32(data["countriesList"]) };
            _context.Entry(province.Country).State = System.Data.Entity.EntityState.Unchanged;
            _context.Provinces.Add(province);
            _context.SaveChanges();
            return RedirectToAction("ShowProvince", "Province");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //Show All Provinces
        public ActionResult ShowProvince(int? page)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Province/ShowProvince" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                var pageNumber = page ?? 1;
            var pageSie = 5;
            var ProvinceList = new LocationHandler().getAllPrivinces().ToPagedList(pageNumber, pageSie);
            ViewBag.listProvince = ProvinceList;
            return View(ViewBag.listProvince);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //Edid Province

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Province/Edit" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                Province province = new LocationHandler().getProvinceById(id);
            ViewBag.countryList = new SelectList(_context.Countries, "ID", "Name", province.Country.ID);

            //ViewBag.Province = province;
            return View(province);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Edit(Province p)
        {
            //Adding Query String 
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if (currentUser == null) return RedirectToAction("Login", "User", new { returnurl = "Province/Edit" });
            if (currentUser.IsRole(WebUtil.ADMIN_ROLE))
            {
                Province pr = _context.Provinces.Where(x => x.ID == p.ID).FirstOrDefault();
            //pr.ID = p.ID;
            pr.Name = p.Name;
            pr.Country = new Country { ID = Convert.ToInt32(p.Country.ID) };

            _context.Entry(pr.Country).State = System.Data.Entity.EntityState.Unchanged;

            //_context.Entry(pr).State = System.Data.Entity.EntityState.Modified;
            //_context.Provinces.Attach(pr);
            _context.SaveChanges();
            //Province pro = new Province();
            //Province pro= new LocationHandler().getProvinceById(p.ID);
            //pro.ID = p.ID;
            //pro.Name = p.Name;
            //pro.Country = new Country { ID = p.Country.ID,Name=p.Country.Name };
            ////pro.Name = p.Name;
            ////pro.Country = new Country { ID =p.ID };
            //DemoContext cc = new DemoContext();
            //cc.Entry(pro.Country).State = System.Data.Entity.EntityState.Unchanged;
            ////cc.Provinces.Attach(pro);
            //cc.Entry(p).State = System.Data.Entity.EntityState.Modified;
            //cc.Provinces.Attach(p);

            //cc.SaveChanges();

            //Province pro = new Province();
            //pro.Name = p.Name;
            //pro.Country = new Country { ID = Convert.ToInt32(p.ID) };
            //_context.Entry(pro.Country).State = System.Data.Entity.EntityState.Unchanged;
            //_context.Entry(p).State = System.Data.Entity.EntityState.Modified;
            //  _context.SaveChanges();

            return RedirectToAction("ShowProvince", "Province");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Province province = _context.Provinces.Where(x => x.ID.Equals(id)).SingleOrDefault();

            _context.Provinces.Remove(province);
            _context.SaveChanges();
            return RedirectToAction("ShowProvince", "Province");
        }

    }
}