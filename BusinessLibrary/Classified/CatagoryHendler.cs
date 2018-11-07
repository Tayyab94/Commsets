using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLibrary;
using System.Data.Entity;

namespace BusinessLibrary.Classified
{
   public class CatagoryHendler
    {

        //Get All Catagories
        public List<Catagory> getAllCatagories()
        {
            using (DemoContext _context= new DemoContext())
            {
                return (from cat in _context.Catagories select cat).ToList();
            }
        }

        //Get Catagory By id
        public Catagory getCatagorieById(int id)
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from cat in _context.Catagories where cat.ID==id  select cat).SingleOrDefault();
            }
        }

        //Get SubCatagories
        public List<SubCatagory> GetallSubtCatagorie()
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from ct in _context.SubCatagories .Include("Catagory") select ct).ToList();
            }
        }


        public List<SubCatagory> getSubCatagoriesById(int id)
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from scat in _context.SubCatagories.Include("Catagory") where scat.ID == id select scat).ToList();
            }
        }
        public SubCatagory getSubCatagoryById(int id)
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from s in _context.SubCatagories.Include("Catagory") where s.ID==id select s).SingleOrDefault();
            }
        }

        public List<SubCatagory> getSubCatagoriesByCatagoryId(Catagory c)
        {
            using (DemoContext _context = new DemoContext())
            {
                //// hosla rakh ik minuet
                return _context.SubCatagories.Include(x => x.Catagory).Where(x => x.Catagory.ID == c.ID).ToList();
                //return (from scat in _context.SubCatagories
                //        .Include("Catagory") where scat.Catagory.ID == c.ID select scat).ToList();
            }
        }
    }
}
