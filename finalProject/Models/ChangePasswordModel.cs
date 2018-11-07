using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finalProject.Models
{
    public class ChangePasswordModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage ="Current Password is missing..!")]
       public string CurrentPass { get; set; }

        [Required(ErrorMessage ="Please Enter New Password")]
        [StringLength(10,MinimumLength =5,ErrorMessage ="Password should be min 5 or Max 10 Charators")]
        public string NewPass { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("NewPass",ErrorMessage ="Password and confirm password doesnot matched")]
        public string ConfPassword { get; set; }

       // [Required(ErrorMessage ="Enter Email Address")]
        public string CurrentEmail { get; set; }
       //public int c;
    }
}