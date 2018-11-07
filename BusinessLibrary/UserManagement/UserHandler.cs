using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLibrary;
using System.Data.Entity;

namespace BusinessLibrary.UserManagement
{


    public class UserHandler
    {
        //Get All Users
        public List<User> getAllUsers()
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from u in _context.Users.Include("Role").Include("City.Province.Country") select u).ToList();
            }
        }

        //Get Uses By id
        public User getUserById(int id)
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from u in _context.Users.Include("Role").Include("City.Province.Country") where u.ID == id select u).SingleOrDefault();
            }
        }

        //Get Uses By login Id and  password
        public User getLoginUser(string loginId,string pass)
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from u in _context.Users.Include("Role").Include("City.Province.Country") where u.LoginId==loginId && u.Password==pass select u).SingleOrDefault();
            }
        }


        //Get All User Roles
        public List<Role> getallRoles()
        {
            using (DemoContext _context = new DemoContext())
            {
                return (from r in _context.Roles select r).ToList();
            }
        }

        public void Add(User obj)
        {
            using (DemoContext _context = new DemoContext())
            {
                _context.Entry(obj.City).State = EntityState.Unchanged;
                _context.Entry(obj.Role).State = System.Data.Entity.EntityState.Unchanged;
                _context.Users.Add(obj);
                _context.SaveChanges();
            }
        }
    }
}
