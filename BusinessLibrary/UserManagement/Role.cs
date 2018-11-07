using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLibrary.UserManagement
{
   public class Role
    {
        public int ID { get; set; }


        private string name;

        [Required(ErrorMessage ="Please Enter Role")]
        [MaxLength(50)]
        [Column(TypeName ="varchar")]

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
