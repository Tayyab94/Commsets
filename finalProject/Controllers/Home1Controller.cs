using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using finalProject.Models;
using BusinessLibrary.Classified;
using System.Web.Mvc;

namespace finalProject.Controllers
{
    public class Home1Controller : Controller
    {
        // GET: Home1
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.NewAdvs = new AdvertisementHandler().GetLastetAdvertisement().ToAdvertiseSumModel();
            return View();
        }

        [HttpGet]
        public ActionResult CatById(int id)
        {
            //Catagory c = new CatagoryHendler().getCatagorieById(id);
            //ViewBag.CataName = c.Name;
            ViewBag.Cat = new AdvertisementHandler().ListAdvertisemetByCataId(new Catagory { ID = id }).ToAdvertiseSumModel();

            return View();
        }
    }
}