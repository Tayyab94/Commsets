using BusinessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLibrary.Classified;
namespace finalProject.Controllers
{
    public class CatagoryController : Controller
    {
        // GET: Catagory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult showAllCatagories()
        {

            List<Catagory> catagoriesList = new CatagoryHendler().getAllCatagories();
            return View(catagoriesList);
          
        }
        // GET: Catagory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Catagory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catagory/Create
        [HttpPost]
        public ActionResult Create(Catagory collection)
        {
           
          
            try
            {
                // TODO: Add insert logic here
                
                DemoContext _context = new DemoContext();
                _context.Catagories.Add(collection);
                _context.SaveChanges();
            
                     return View();
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Catagory/Edit/5
        public ActionResult EditCatagory(int id)
        {
            Catagory cat = new DemoContext().Catagories.Where(x => x.ID == id).SingleOrDefault();

            return View(cat);
        }

        // POST: Catagory/Edit/5
        [HttpPost]
        public ActionResult EditCatagory(Catagory obj)
        {
            DemoContext _context = new DemoContext();
            _context.Entry<Catagory>(obj).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("showAllCatagories", "Catagory");
        }

        // GET: Catagory/Delete/5
       
        // POST: Catagory/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (DemoContext _context = new DemoContext())
            {
                Catagory cat = _context.Catagories.Where(x => x.ID == id).SingleOrDefault();
                _context.Catagories.Remove(cat);
                _context.SaveChanges();
                return RedirectToAction("showAllCatagories", "Catagory");
            }
        }
    }
}
