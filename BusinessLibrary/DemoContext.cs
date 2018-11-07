using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BusinessLibrary.UserManagement;
using BusinessLibrary.Classified;
namespace BusinessLibrary
{
   public class DemoContext:DbContext
    {
        public DemoContext():base("pro")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Ciities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get;set; }

        public DbSet<Catagory> Catagories { get; set; }

        public DbSet<SubCatagory> SubCatagories { get; set; }

        public DbSet<AdvertisementType> Advtypes { get; set; }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<AdvertisementImages> AdvertisementImages { get; set; }
        public DbSet<AdvertisementStatus> AdvertisementStatuses { get; set; }


    }
}
