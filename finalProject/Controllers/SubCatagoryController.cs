using BusinessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLibrary.Classified;

namespace finalProject.Controllers
{
    public class SubCatagoryController : Controller
    {
        // GET: SubCatagory
        [HttpGet]
        public ActionResult ShowAllSubCatagories()
        {
            List<SubCatagory> SbCatagoriesList = new CatagoryHendler().GetallSubtCatagorie();
            return View(SbCatagoriesList);
        }

        public ActionResult AddSubCatagory()
        {
            DemoContext _context = new DemoContext();
            ViewBag.catagoryList = new SelectList(_context.Catagories, "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddSubCatagory(FormCollection data)
        {
            using (DemoContext _Context = new DemoContext())
            {
                SubCatagory sCat = new SubCatagory();
                sCat.Name = data["sCataName"];
                sCat.Catagory = new Catagory { ID = Convert.ToInt32(data["catagoryList"]) };
                _Context.Entry(sCat.Catagory).State = System.Data.Entity.EntityState.Unchanged;
                _Context.SubCatagories.Add(sCat);
                _Context.SaveChanges();
                return RedirectToAction("AddSubCatagory");
            }
        }

        [HttpGet]
        public ActionResult EditSubCatagories(int id)
        { DemoContext _context = new DemoContext();
            SubCatagory sCata = new CatagoryHendler().getSubCatagoriesById(id).SingleOrDefault();
            ViewBag.CatagoryList = new SelectList(_context.Catagories, "ID", "Name", sCata.Catagory.ID);
            return View(sCata);
        }
        [HttpPost]
        public ActionResult EditSubCatagories(SubCatagory obj)
        {
            DemoContext _context = new DemoContext();
            SubCatagory sCat = _context.SubCatagories.Where(x => x.ID.Equals(obj.ID)).FirstOrDefault();
            sCat.Name = obj.Name;
            sCat.Catagory = new Catagory { ID = Convert.ToInt32(obj.Catagory.ID) };
            _context.Entry(sCat.Catagory).State = System.Data.Entity.EntityState.Unchanged;
           
            _context.SaveChanges();
            return RedirectToAction("ShowAllSubCatagories");
        }

        [HttpGet]
        public ActionResult DelSubCatagories(int id)
        {
            DemoContext _context = new DemoContext();
            SubCatagory sCat = _context.SubCatagories.Where(x => x.ID.Equals(id)).FirstOrDefault();
            _context.SubCatagories.Remove(sCat);
            _context.SaveChanges();
            return RedirectToAction("ShowAllSubCatagories");
        }
    }
}