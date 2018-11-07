using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finalProject.Models
{
    public class LoginModel
    {
        private string  loginid;
        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",ErrorMessage ="Invalid Email Address.")]
        [Display(Name ="Login Id")]

        public string  LoginId
        {
            get { return loginid; }
            set { loginid = value; }
        }

        private string password;
        [Required]
        [DataType(DataType.Password)]
        [StringLength(25,ErrorMessage ="Your Password should be Min 7 or Max 25 Charactors",MinimumLength =7)]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }


    }
}