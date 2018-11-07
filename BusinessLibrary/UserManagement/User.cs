using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLibrary.UserManagement
{
   public class User
    {
        public int ID { get; set; }

        private string fullName;
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }


        private string loginId;     
        [Required]
        [MaxLength(50)]
        [Column(TypeName="varchar")]
        
        public string LoginId
        {
            get { return loginId; }
            set { loginId = value; }
        }

        private string password;

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar")]

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string verificationCode;

       
        [MaxLength(30)]
        [Column(TypeName = "varchar")]

        public string VerificationCode
        {
            get { return verificationCode; }
            set { verificationCode = value; }
        }


        private string seccurityQuestion;

        [MaxLength(250)]
        [Column(TypeName = "varchar")]

        public string SecurityQuestion
        {
            get { return seccurityQuestion; }
            set { seccurityQuestion = value; }
        }

        private string securityAns;

        [MaxLength(50)]
        [Column(TypeName = "varchar")]

        public string SecurityAns
        {
            get { return securityAns; }
            set { securityAns = value; }
        }

        private string contactNo;

        [Required]
        [MaxLength(25)]
        [Column(TypeName = "varchar")]

        public string ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }

        private string address;

        [Required]
        [MaxLength(250)]
        [Column(TypeName = "varchar")]

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public virtual City City { get; set; }

        [MaxLength(250)]
        [Column(TypeName = "varchar")]

        public string EmailID { get; set; }

        [MaxLength(250)]
        [Column(TypeName = "varchar")]

        public string ImgURL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public Nullable<DateTime> DOB { get; set; }

        public Nullable<bool> IsActive { get; set; }
        
        [Required]
        public virtual Role Role { get; set; }

        public bool IsRole(int id)
        {
            return Role != null && Role.ID == id; 
        }
    }
}
