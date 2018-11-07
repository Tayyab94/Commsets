using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLibrary
{
   public class Country:IListable
    {
        [Key]
        public int ID { get; set; }

        
        public Nullable<int> Code { get; set; }

        private string name;
        [Required(ErrorMessage ="County Name is Required")]
        [MaxLength(50)]
        [Column(TypeName ="varchar")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


    }
}
