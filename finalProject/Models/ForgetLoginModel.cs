using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace finalProject.Models
{
    public class ForgetLoginModel
    {
        public int UserId { get; set; }

        public string VerificationCode { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "Password should be min 5 or Max 10 Charators")]
        //[Column(TypeName = "varchar")]
        public string NewPass { get; set; }
        [Compare("NewPass", ErrorMessage = "Password and confirm password doesnot matched")]
        public string ConfPassword { get; set; }
    }
}