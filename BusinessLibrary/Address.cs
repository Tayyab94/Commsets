using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLibrary
{
   public  class Address
    {
        public int Address_ID { get; set; }
        private string streetAddress;
        [Required(ErrorMessage = "Province Name is Required")]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string StreetAddress
        {
            get { return streetAddress; }
            set { streetAddress = value; }
        }

        public virtual City City { get; set; }
    }
}
