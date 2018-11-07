using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessLibrary.UserManagement;

namespace BusinessLibrary.Classified
{
   public class Advertisement
    {
        public Advertisement()
        {
            Images = new List<AdvertisementImages>();
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Please Enter the Title")]
        [Column(TypeName = "varchar")]
        [MaxLength(300)]
        public string Title { get; set; }

        [Column(TypeName = "varchar")]
        public string Description { get; set; }

        public float price { get; set; }

        public DateTime PostedOn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime VilidUpTo { get; set; }

        [Required]

        public virtual User User { get; set; }

        [Required]
        public virtual City City { get; set; }


        [Required]
        public virtual SubCatagory SubCatagory { get; set; }
            
        [Required]
        public virtual AdvertisementType Type { get; set; }

        [Required]
        public virtual AdvertisementStatus Status { get; set; }

        public ICollection<AdvertisementImages> Images { get; set; }

    }
}
