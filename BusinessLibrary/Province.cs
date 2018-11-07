using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary
{
   public class Province:IListable
    {
        public int ID { get; set; }
        private string name;

        [Required(ErrorMessage ="Province Name is Required")]
        [MaxLength(50)]
        [Column(TypeName="varchar")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public virtual Country Country { get;
            set;
        }
    }
}
