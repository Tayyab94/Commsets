using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLibrary.Classified
{
   public class AdvertisementImages
    {
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(300)]
        public string Url { get; set; }


       
        [Column(TypeName = "varchar")]
        [MaxLength(100)]
        public string Caption { get; set; }

        public int Priority { get; set; }

        public virtual Advertisement Advertisement { get; set; }
   
    }
}
