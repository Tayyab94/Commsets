using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLibrary;
using System.Data.Entity;
using BusinessLibrary.Classified;
using finalProject.Models;
using BusinessLibrary.UserManagement;
using PagedList;
using System.Data.Entity.Core.Objects;

namespace finalProject.Areas.AdminSide.Controllers
{
    public class HomeController : Controller
    {
        // GET: AdminSide/Home
        public ActionResult Index(int? page)
        {
            User currentUser = (User)Session[WebUtil.CURRENT_USER];
            if(currentUser==null)
            {
                return RedirectToAction("Login", "User", new { returnurl = "AdminSide/Home/Index" });
            }
            var pageNumber = page ?? 1;
            var pageSize = 5;
            DemoContext _context = new DemoContext();
            var usersList = new UserHandler().getAllUsers().ToPagedList(pageNumber, pageSize);
           // ViewBag.U_list = usersList;
           
            return View(usersList);
        }

        [HttpGet]
        public  ActionResult PenddingPost()
        {
            DemoContext _context = new DemoContext();
            //ViewBag.lstAdv = new AdvertisementHandler().GetPenddingAdvertisement().ToAdvertiseSumModel();
            List<Advertisement> AdvList = new AdvertisementHandler().GetPenddingAdvertisement().ToList();
            return View(AdvList);
        }

        public ActionResult DetailPost(int id)
        {
            Advertisement objAdvertisement = new AdvertisementHandler().GetDetailAdvertisement(id);
            return View(objAdvertisement);
        }
        public ActionResult OldPost()
        {
            DemoContext _context = new DemoContext();
            var objAvd = _context.Advertisements.Include(x => x.City.Province.Country).Include(x => x.Images).Include(x => x.Status).Include(x => x.SubCatagory.Catagory).Include(x => x.Type).Include(x => x.User).Where(x => EntityFunctions.AddMonths(x.PostedOn, 2) > DateTime.Now).ToList();
            return View(objAvd);
        }

    }
}