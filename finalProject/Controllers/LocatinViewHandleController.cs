using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalProject.Models;
using BusinessLibrary;
using BusinessLibrary.Classified;


namespace finalProject.Controllers
{
    public class LocatinViewHandleController : Controller
    {
        // GET: LocatinViewHandle
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Province(int id)
        {
            DDLViewModel model = new DDLViewModel();
            model.Name = "Province";
            model.Caption = "- Select Province -";
            model.Glyphicon = "glyphicon glyphicon-map-marker";
            model.values = new LocationHandler().getProvincesBtCountry(new Country { ID = id }).ToSelectList();
            return PartialView("~/Views/Shared/_DDLView.cshtml", model);
        }
        public ActionResult City(int id)
        {
            DDLViewModel model = new DDLViewModel();
            model.Name = "City";
            model.Caption = "- Select City -";
            model.Glyphicon = "glyphicon glyphicon-map-marker";
            model.values = new LocationHandler().getAllCityByProvinceId(new Province { ID = id }).ToSelectList();
            return PartialView("~/Views/Shared/_DDLView.cshtml", model);
        }

        public ActionResult SubCatagory(int id)
        {
            DDLViewModel model = new DDLViewModel();
            model.Name = "SubCatagory";
            model.Caption = "- Select SubCatagory -";
            model.Glyphicon = "glyphicon glyphicon-tag";
            model.values = new CatagoryHendler().getSubCatagoriesByCatagoryId(new Catagory { ID = id }).ToSelectList();
            return PartialView("~/Views/Shared/_DDLView.cshtml", model);
        }
    }
}