using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary
{
   public class LocationHandler
    {
        DemoContext _context = new DemoContext();
        public List<Country> getAllCountries()
        {
            return (from C in _context.Countries select C).ToList();
        }


        //GEt Country By Id for the edit or Show Method
        public Country getCountryById(int id)
        {
            return (from C in _context.Countries where C.ID == id select C).FirstOrDefault();
        }


        //Get ALl Province
        public List<Province> getAllPrivinces()
        {
            return (from p in _context.Provinces .Include("Country") select p).ToList();
        }


        //Get Province By Country's Id
        public List<Province>  getProvincesBtCountry(Country c)
        {
            using (_context)
            {
                return (from pr in _context.Provinces .Include("Country")
                        where pr.Country.ID == c.ID
                        select pr).ToList();
            }
        }


        //Get PRovine By Id for the Edit or Delete show Function 
        public Province getProvinceById(int id)
        {
            using (_context)
            {
                return (from pr in _context.Provinces 
                        .Include("Country")
                        where pr.ID == id
                        select pr).FirstOrDefault();
            }
        }

        public List<City> getAllCities()
        {
            return (from c in _context.Ciities .Include("Province") orderby(c.Name) select c).ToList();
        }

        public City getCityById(int id)
        {
            using (_context)
            {
                return (from c in _context.Ciities
                        .Include("Province")
                        where c.ID == id
                        select c).FirstOrDefault();
            }
        }

        public List<City> getAllCityByProvinceId(Province p)
        {
            using (_context)
            {
                return (from c in _context.Ciities.Include("Province.Country")
                        where (c.Province.ID == p.ID)
                        select c).ToList();
            }
        }
    }
}
