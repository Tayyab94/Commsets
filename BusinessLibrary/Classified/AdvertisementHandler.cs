using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BusinessLibrary.Classified
{
    public class AdvertisementHandler
    {
        public void Add(Advertisement obj)
        {
            using (DemoContext _context = new DemoContext())
            {
                _context.Entry(obj.City).State = EntityState.Unchanged;
                _context.Entry(obj.Type).State = EntityState.Unchanged;
                _context.Entry(obj.SubCatagory).State = EntityState.Unchanged;
                _context.Entry(obj.Status).State = EntityState.Unchanged;
                _context.Entry(obj.User).State = EntityState.Unchanged;
                _context.Advertisements.Add(obj);
                _context.SaveChanges();
            }

        }

        public List<Advertisement> ListAdvertisemetByCataId(Catagory obj)
        {
            DemoContext _context = new DemoContext();
            using (_context)
            {
                return (from ad in _context.Advertisements.Include(x => x.City.Province.Country).Include(x => x.SubCatagory.Catagory)
                        .Include(x => x.User).Include(x => x.Status).Include(x => x.Images).Include(x => x.Type).Where(x => x.SubCatagory.Catagory.ID.Equals(obj.ID) && x.Status.ID==1)
                        select ad).ToList();
            }
        }

        public List<Advertisement> GetAdvertisemetByCataId(Catagory obj)
        {
            DemoContext _context = new DemoContext();
            using (_context)
            {
                return (from ad in _context.Advertisements.Include(x => x.City.Province.Country).Include(x => x.SubCatagory.Catagory)
                        .Include(x => x.User).Include(x => x.Status).Include(x => x.Images).Include(x => x.Type).Where(x => x.SubCatagory.Catagory.ID.Equals(obj.ID))
                        select ad).ToList();
            }
        }


        //

        public List<Advertisement> GetLastetAdvertisement()
        {
            DemoContext _context = new DemoContext();
            using (_context)
            {
                return (from adv in _context.Advertisements
                        .Include(a => a.SubCatagory.Catagory)
                        .Include(a => a.City.Province.Country)
                        .Include(a => a.User)
                        .Include(a => a.Images)
                        .Include(a => a.Status)
                        .Include(a => a.Type)
                        .OrderByDescending(x=>x.PostedOn)
                        //.OrderBy(x => x.PostedOn)
                        .Where(x=>x.Status.ID==1)
                        
                        select adv
                        ).Take(12).ToList();
            }
        }

        //Recommended Advertisement Selected
        public List<Advertisement> GetRecomendAdvertisement(int id)
        {
            DemoContext _context = new DemoContext();
            using (_context)
            {
                return (from adv in _context.Advertisements
                        .Include(a => a.SubCatagory.Catagory)
                        .Include(a => a.City.Province.Country)
                        .Include(a => a.User)
                        .Include(a => a.Images)
                        .Include(a => a.Status)
                        .Include(a => a.Type)
                        .OrderByDescending(x => x.PostedOn)
                            //.OrderBy(x => x.PostedOn)
                            .Where(a=>a.SubCatagory.Catagory.ID==id)
                        select adv
                        ).Take(5).ToList();
            }
        }

        public Advertisement GetDetailAdvertisement(int id)
        {
            DemoContext _context = new DemoContext();
            using (_context)
            {
                return _context.Advertisements
                       .Include(a => a.SubCatagory.Catagory)
                       .Include(a => a.City.Province.Country)
                       .Include(a => a.User)
                       .Include(a => a.Images)
                       .Include(a => a.Status)
                       .Include(a => a.Type).SingleOrDefault(x => x.ID == id);
                //return (from ad in _context.Advertisements
                //        .Include(a => a.SubCatagory.Catagory)
                //        .Include(a => a.City.Province.Country)
                //        .Include(a => a.User)
                //        .Include(a => a.Images)
                //        .Include(a => a.Status)
                //        .Include(a => a.Type)

                //            //.OrderBy(x => x.PostedOn)
                //        where (ad.ID.Equals(id))
                //        select ad
                //        ).SingleOrDefault();
            }
        }

        public List<Advertisement> GetUserAdvertisement(int id)
        {
            DemoContext _context = new DemoContext();
            using (_context)
            {
                return (from ad in _context.Advertisements
                        .Include(a => a.SubCatagory.Catagory)
                        .Include(a => a.City.Province.Country)
                        .Include(a => a.User)
                        .Include(a => a.Images)
                        .Include(a => a.Status)
                        .Include(a => a.Type)

                            //.OrderBy(x => x.PostedOn)
                        where (ad.User.ID.Equals(id))
                        select ad
                        ).ToList();
            }
        }

        public List<Advertisement> GetPenddingAdvertisement()
        {
            DemoContext _context = new DemoContext();
            using (_context)
            {
                return (from adv in _context.Advertisements
                        .Include(a => a.SubCatagory.Catagory)
                        .Include(a => a.City.Province.Country)
                        .Include(a => a.User)
                        .Include(a => a.Images)
                        .Include(a => a.Status)
                        .Include(a => a.Type)
                        .OrderByDescending(x => x.PostedOn)
                            //.OrderBy(x => x.PostedOn)
                            .Where(a => a.Status.ID == 2)
                        select adv
                        ).ToList();
            }
        }

    }
}
