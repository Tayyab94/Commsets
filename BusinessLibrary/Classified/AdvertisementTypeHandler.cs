using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Classified
{
   public class AdvertisementTypeHandler
    {
        public List<AdvertisementType> GetAllAdvertisementTypes()
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from c in _context.Advtypes select c).ToList();
            }
        }
    }
}
